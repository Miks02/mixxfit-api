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
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Features.Workouts.CreateWorkout;
using MixxFit.API.Infrastructure.Cloudinary;
using MixxFit.API.Infrastructure.Exceptions;
using MixxFit.API.Infrastructure.Extensions;
using MixxFit.API.Infrastructure.Hangfire;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Infrastructure.Security;
using MixxFit.API.Infrastructure.Storage;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddScoped<IFileService, CloudinaryFileStorage>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(filter: descriptor => descriptor.ValidatorType != typeof(SetEntryValidator));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();

builder.Services.AddCloudinary(builder.Configuration);


builder.Services.InjectHandlers();

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

builder.Services.AddHangfireSetup(builder.Configuration);
builder.Services.AddRecurringJobs();

var app = builder.Build();

app.UseHangfireDashboardWithAuthorization();
app.UseRecurringJobs();

app.UseStaticFiles();

app.UseForwardedHeaders();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms (User: {UserId})";

    options.EnrichDiagnosticContext = (context, httpContext) =>
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
        context.Set("UserId", userId);
    };
});

app.MapEndpoints();

app.MapMethods("api/health", ["GET", "HEAD"], () => new { Status = "Healthy", Date = DateTime.UtcNow });

app.UseRateLimiter();

app.UseHttpsRedirection();


app.Run();
