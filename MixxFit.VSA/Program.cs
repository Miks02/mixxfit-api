using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using CloudinaryDotNet;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Domain.Entities;
using MixxFit.VSA.Infrastructure.Configuration;
using MixxFit.VSA.Infrastructure.Exceptions;
using MixxFit.VSA.Infrastructure.Extensions;
using MixxFit.VSA.Infrastructure.Persistence;
using MixxFit.VSA.Infrastructure.Security;
using MixxFit.VSA.Infrastructure.Storage;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Token"]!))
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();

builder.Services.AddAuthorization();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICookieProvider, CookieProvider>();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddScoped<IFileService, LocalFileStorage>();

builder.Services.InjectHandlers();

var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinaryOptions>();
var account = new Account(
    cloudinarySettings!.CloudName,
    cloudinarySettings.ApiKey,
    cloudinarySettings.ApiSecret
);
var cloudinary = new Cloudinary(account);

builder.Services.AddSingleton(cloudinary);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });

    options.AddPolicy("ProdCors", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("https://vitalops-web.onrender.com")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });

});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        var hasMetadata = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter);

        var detailMessage = hasMetadata
            ? $"Request limit reached, try again after {retryAfter.TotalSeconds} seconds"
            : "Request limit reached, please try again later.";

        var problem = new ProblemDetails()
        {
            Title = "Too many requests",
            Detail = detailMessage,
            Status = options.RejectionStatusCode,
            Instance = context.HttpContext.Request.Path
        };

        if (hasMetadata)
        {
            context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString();
            problem.Extensions["RetryAfter"] = retryAfter.TotalSeconds;
        }

        await context.HttpContext.Response.WriteAsJsonAsync(problem, token);
    };
    
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        var partitionKey = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(partitionKey))
            partitionKey = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        return RateLimitPartition.GetFixedWindowLimiter(partitionKey, _ => new FixedWindowRateLimiterOptions()
        {
            PermitLimit = 100,
            Window = TimeSpan.FromSeconds(45)
        });

    });

});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseCors("AllowCors");
}
else
{
    app.UseCors("ProdCors");
}

app.MapEndpoints();

app.MapMethods("/health", ["GET", "HEAD"], () => new { Status = "Healthy", Date = DateTime.UtcNow });

app.UseHttpsRedirection();

app.Run();
