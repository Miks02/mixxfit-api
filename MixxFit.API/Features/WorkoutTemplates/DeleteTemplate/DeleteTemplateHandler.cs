using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WorkoutTemplates.DeleteTemplate;

public class DeleteTemplateHandler(AppDbContext context) : IHandler
{
    public async Task<Result> Handle(string userId, int templateId, CancellationToken ct)
    {
        var fitnessProfileId = await context.FitnessProfiles
            .Where(fp => fp.UserId == userId)
            .Select(fp => (int?)fp.Id)
            .FirstOrDefaultAsync(ct);
        
        if(fitnessProfileId is null)
            return Result.Failure(UserError.NotFound(userId));
        
        var templateToDelete = await context.WorkoutTemplates
            .Where(wt => wt.Id == templateId && wt.FitnessProfileId == fitnessProfileId)
            .FirstOrDefaultAsync(ct);
        
        if(templateToDelete is null)
            return Result.Failure(GeneralError.NotFound($"Workout template with id {templateId} was not found for user {userId}"));

        context.WorkoutTemplates.Remove(templateToDelete);
        await context.SaveChangesAsync(ct);
        
        return Result.Success();
    }
}