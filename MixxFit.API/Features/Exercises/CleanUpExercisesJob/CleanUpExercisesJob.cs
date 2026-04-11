using Microsoft.EntityFrameworkCore;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Exercises.CleanUpExercisesJob
{
    public class CleanUpExercisesJob(AppDbContext context, ILogger<CleanUpExercisesJob> logger)
    {
        public async Task Execute()
        {
            logger.LogInformation("Background job has started...");

            int affectedRows = await context.Exercises
                .IgnoreQueryFilters()
                .Where(e => e.IsDeleted && e.ExerciseEntries.Count == 0)
                .ExecuteDeleteAsync();

            if (affectedRows > 0)
            {
                logger.LogInformation("Background job has completed the task successfully. Deleted exercises: {delEx}", affectedRows);
                return;
            }

            logger.LogInformation("Background job has completed the task successfully.");

        }
    }
}
