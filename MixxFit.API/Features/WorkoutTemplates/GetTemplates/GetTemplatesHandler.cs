using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WorkoutTemplates.GetTemplates;

public class GetTemplatesHandler(AppDbContext context) : IHandler
{
    public async Task<IReadOnlyList<GetTemplatesResponse>> Handle(string userId, CancellationToken cancellationToken)
    {
        return await context.WorkoutTemplates
            .Where(wt =>  wt.FitnessProfileId == null || wt.FitnessProfile!.UserId == userId)
            .Select(wt => new GetTemplatesResponse
            {
                Id = wt.Id,
                Name = wt.Name,
                Notes = wt.Notes,
                IsSystem = wt.FitnessProfileId == null,
                Exercises = wt.WorkoutTemplateExercises.Select(wte => new TemplateExerciseDto
                {
                    ExerciseId = wte.ExerciseId,
                    SetCount = wte.SetCount,
                    Order = wte.Order
                }).OrderBy(e => e.Order).ToList()
            })
            .ToListAsync(cancellationToken);
    }
}