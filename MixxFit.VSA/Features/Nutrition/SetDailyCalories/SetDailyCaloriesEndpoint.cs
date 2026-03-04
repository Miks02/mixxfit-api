using System.Net;
using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Nutrition.SetDailyCalories;

public class SetDailyCaloriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/nutrition/daily-calories", async (
                SetDailyCaloriesRequest request,
                SetDailyCaloriesHandler handler,
                ICurrentUserProvider userProvider,
                CancellationToken cancellationToken = default) =>
            {
                var result = await handler.Handle(userProvider.GetCurrentUserId(), request, cancellationToken);

                return result.ToTypedResult(HttpStatusCode.Created);
            })
            .WithTags("Nutrition")
            .RequireAuthorization()
            .Produces<SetDailyCaloriesResponse>(StatusCodes.Status201Created)
            .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

    }
}
