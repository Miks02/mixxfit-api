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
using MixxFit.API.Infrastructure.Cors;
using MixxFit.API.Infrastructure.Exceptions;
using MixxFit.API.Infrastructure.Extensions;
using MixxFit.API.Infrastructure.Hangfire;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Infrastructure.RateLimiting;
using MixxFit.API.Infrastructure.Security;
using MixxFit.API.Infrastructure.Storage;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddSecurity(builder.Configuration);


builder.Services.AddHttpContextAccessor();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>(filter: descriptor => descriptor.ValidatorType != typeof(SetEntryValidator));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();

builder.Services.AddCloudinary(builder.Configuration);

builder.Services.InjectHandlers();

builder.Services.AddCorsPolicies();

builder.Services.AddGlobalRateLimiter();

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
