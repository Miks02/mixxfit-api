using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static void InjectHandlers(this IServiceCollection services)
    {
        var handlers = typeof(Program).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IHandler)));

        foreach (var handler in handlers)
            services.AddScoped(handler);
    }
}