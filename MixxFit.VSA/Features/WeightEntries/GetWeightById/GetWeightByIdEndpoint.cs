using Microsoft.AspNetCore.Http.HttpResults;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightById;

public class GetWeightByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("weight-entries/{id:int}", async Task<Results<Ok<GetWeightByIdResponse>, NotFound>> (
                int id,
                GetWeightByIdHandler handler,
                ICurrentUserProvider userProvider,
                CancellationToken cancellationToken = default) =>
            {
                var weightEntry = await handler.Handle(userProvider.GetCurrentUserId(), id, cancellationToken);

                if (weightEntry is null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(weightEntry);
            })
            .WithTags("WeightEntries")
            .RequireAuthorization();

    }
}