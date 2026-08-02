using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Infrastructure.Extensions;
using MixxFit.API.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

builder.Services.Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database migrations.");
    }
}

app.UseStaticFiles();

app.UseForwardedHeaders();

app.UseExceptionHandler();

switch (app.Environment.EnvironmentName)
{
    case "Development":
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
        app.UseCors("DevCors");
        break;
    }
    case "Staging":
    {
        app.UseCors("StagingCors");
        break;
    }
    case "Production":
    {
        app.UseCors("ProductionCors");
        break;
    }
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
