using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Exercises.Shared;
using MixxFit.API.Features.WorkoutTemplates.Common;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WorkoutTemplates.CreateTemplate;

public class CreateTemplateHandler(AppDbContext context) : IHandler
{
    public async Task<Result<CreateTemplateResponse>> Handle(string userId, CreateTemplateRequest request, CancellationToken ct)
    {
        var fitnessProfileId = await context.FitnessProfiles
            .Where(fp => fp.UserId == userId)
            .Select(fp => (int?)fp.Id)
            .FirstOrDefaultAsync(ct);
        
        if(fitnessProfileId is null)
            return Result<CreateTemplateResponse>.Failure(UserError.NotFound(userId));
        
        var validationResult = await ValidateData(fitnessProfileId.Value, request, ct);
        
        if(!validationResult.IsSuccess)
            return Result<CreateTemplateResponse>.Failure(validationResult.Errors.ToArray());
        
        var newTemplate = new WorkoutTemplate
        {
            Name = request.Name,
            Notes = request.Notes,
            FitnessProfileId = fitnessProfileId,
            WorkoutTemplateExercises = request.Exercises.Select((e, index) => new WorkoutTemplateExercise
            {
                ExerciseId = e.ExerciseId,
                SetCount = e.SetCount,
                Order = index + 1
            }).ToList()
        };
        
        context.WorkoutTemplates.Add(newTemplate);
        await context.SaveChangesAsync(ct);

        var response = new CreateTemplateResponse
        {
            Id = newTemplate.Id,
            Notes = newTemplate.Notes,
            Name = newTemplate.Name,
            Exercises = newTemplate.WorkoutTemplateExercises.Select(wte => new TemplateExerciseDto
            {
                ExerciseId = wte.ExerciseId,
                SetCount = wte.SetCount,
                Order = wte.Order
            }).OrderBy(e => e.Order).ToList()
        };
        
        return Result<CreateTemplateResponse>.Success(response);
    }

    private async Task<Result> ValidateData(int fitnessProfileId, CreateTemplateRequest request, CancellationToken ct)
    {
        var numberOfTemplates = await context.WorkoutTemplates
            .Where(wt => wt.FitnessProfileId == fitnessProfileId)
            .CountAsync(ct);
        
        if(numberOfTemplates == 20)
            return Result.Failure(GeneralError.LimitReached("Limit reached for new templates. Maximum allowed number of templates per user is 20"));
        
        var normalizedTemplateName = request.Name.ToLower().Trim();

        var duplicateTemplate = await context.WorkoutTemplates
            .AnyAsync(wt => wt.FitnessProfileId == fitnessProfileId 
                            && wt.Name.ToLower() == normalizedTemplateName, ct);
        
        if(duplicateTemplate)
            return Result.Failure(GeneralError.Conflict("Workout template with the same name already exists."));
        
        var invalidExerciseIds = await GetInvalidExerciseIds(request.Exercises.Select(e => e.ExerciseId).ToList(), ct);
        
        if(invalidExerciseIds.Count > 0)
            return Result.Failure(ExerciseError.NotFound("One or more requested exercises do not exist. Invalid exercises: " + string.Join(", ", invalidExerciseIds)));

        return Result.Success();
    }

    private async Task<IReadOnlyList<int>> GetInvalidExerciseIds(IReadOnlyList<int> exerciseIds, CancellationToken ct)
    {
        var validIds = await context.Exercises
            .Where(e => exerciseIds.Contains(e.Id))
            .Select(e => e.Id)
            .ToListAsync(ct);

        return exerciseIds.Except(validIds).ToList();
    }
    
    

}