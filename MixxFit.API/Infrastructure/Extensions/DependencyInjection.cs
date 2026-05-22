using FluentValidation;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Features.Workouts.CreateWorkout;
using MixxFit.API.Infrastructure.Cloudinary;
using MixxFit.API.Infrastructure.Cors;
using MixxFit.API.Infrastructure.Exceptions;
using MixxFit.API.Infrastructure.Hangfire;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Infrastructure.RateLimiting;
using MixxFit.API.Infrastructure.Security;

namespace MixxFit.API.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddSecurity(configuration);
        services.AddHttpContextAccessor();
        services.AddCloudinary(configuration);
        //services.AddHangfireSetup(configuration);
        //services.AddRecurringJobs();
        services.AddProblemDetails();
        services.AddValidatorsFromAssemblyContaining<Program>(filter: descriptor => descriptor.ValidatorType != typeof(SetEntryValidator));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddOpenApi();
        services.InjectHandlers();
        services.AddCorsPolicies();
        services.AddGlobalRateLimiter();
    }

    public static void InjectHandlers(this IServiceCollection services)
    {
        var handlers = typeof(Program).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IHandler)));

        foreach (var handler in handlers)
            services.AddScoped(handler);
    }
}