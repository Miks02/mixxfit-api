using MixxFit.API.Common.Interfaces;
using MixxFit.API.Infrastructure.Filters;

namespace MixxFit.API.Infrastructure.Extensions;

public static class EndpointExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/")
            .AddEndpointFilter<ProblemDetailsFilter>()
            .AddEndpointFilter<ValidationFilter>();

        var endpoints = typeof(Program).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IEndpoint).IsAssignableFrom(t));

        foreach (var endpoint in endpoints)
        {
            var instance = Activator.CreateInstance(endpoint) as IEndpoint;
            instance?.MapEndpoint(group);
        }
    }
}