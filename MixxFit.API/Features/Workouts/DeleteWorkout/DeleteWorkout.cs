using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Common.Extensions;

namespace MixxFit.API.Features.Workouts.DeleteWorkout;

public static class DeleteWorkout
{
    public class DeleteWorkoutHandler(AppDbContext context) : IHandler
    {
        public async Task<Result> Handle(string userId, int workoutId, CancellationToken cancellationToken)
        {
            var deleted = await context.Workouts
                .Where(w => w.Id == workoutId && w.UserId == userId)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleted == 0)
                return Result.Failure(GeneralError.NotFound("Workout not found"));

            return Result.Success();
        }
    }

    public class DeleteWorkoutEndpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapDelete("workouts/{id:int}", async (
                    int id,
                    ICurrentUserProvider userProvider,
                    DeleteWorkoutHandler handler,
                    CancellationToken cancellationToken = default) =>
                {
                    var result = await handler.Handle(userProvider.GetCurrentUserId(), id, cancellationToken);
                    return result.ToTypedResult(HttpStatusCode.NoContent);
                })
                .RequireAuthorization()
                .WithTags("Workouts")
                .Produces(StatusCodes.Status204NoContent)
                .Produces<ProblemDetails>(StatusCodes.Status404NotFound);
        }
    }
    
}
