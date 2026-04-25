using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.WorkoutTemplates.Common;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WorkoutTemplates.GetTemplateById;

public class GetTemplateByIdHandler(AppDbContext context) : IHandler
{
    public async Task<Result<GetTemplateByIdResponse>> Handle(string userId, int templateId, CancellationToken cancellationToken)
    {
        var template =  await context.WorkoutTemplates
            .Where(wt => wt.Id == templateId && (wt.FitnessProfileId == null || wt.FitnessProfile!.UserId == userId))
            .Select(wt => new GetTemplateByIdResponse
            {
                Id = wt.Id,
                Name = wt.Name,
                Notes = wt.Notes,
                Exercises = wt.WorkoutTemplateExercises.Select(wte => new TemplateExerciseDto
                {
                    ExerciseId = wte.ExerciseId,
                    SetCount = wte.SetCount,
                    Order = wte.Order
                }).OrderBy(e => e.Order).ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
        
        if(template is null)
            return Result<GetTemplateByIdResponse>.Failure(GeneralError.NotFound($"Workout template with id {templateId} was not found for user {userId}"));
        
        return Result<GetTemplateByIdResponse>.Success(template);
    }
}