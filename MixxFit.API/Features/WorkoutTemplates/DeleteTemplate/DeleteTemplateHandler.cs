using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Domain.Entities.FitnessProfiles;

namespace MixxFit.API.Features.WorkoutTemplates.DeleteTemplate;

public class DeleteTemplateHandler(AppDbContext context) : IHandler
{
    public async Task<Result> Handle(string userId, int templateId, CancellationToken ct)
    {
        var fitnessProfileExists = await context.FitnessProfiles
            .AnyAsync(fp => fp.UserId == userId, ct);
        
        if(!fitnessProfileExists)
            return Result.Failure(FitnessProfileError.NotFound($"Fitness profile for user '{userId}' was not found"));
        
        var templateToDelete = await context.WorkoutTemplates
            .Where(wt => wt.Id == templateId && wt.OwnerId == userId)
            .FirstOrDefaultAsync(ct);
        
        if(templateToDelete is null)
            return Result.Failure(WorkoutTemplateError.NotFound($"Workout template with id '{templateId}' was not found for user '{userId}'"));

        context.WorkoutTemplates.Remove(templateToDelete);
        await context.SaveChangesAsync(ct);
        
        return Result.Success();
    }
}