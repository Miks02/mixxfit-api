using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.WorkoutTemplates.Common;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.WorkoutTemplateExercises;

namespace MixxFit.API.Features.WorkoutTemplates.CreateTemplate;

public class CreateTemplateHandler(AppDbContext context) : IHandler
{
    public async Task<Result<CreateTemplateResponse>> Handle(string userId, CreateTemplateRequest request, CancellationToken ct)
    {
        var fitnessProfileExists = await context.FitnessProfiles
            .AnyAsync(fp => fp.UserId == userId, ct);
        
        if(!fitnessProfileExists)
            return Result<CreateTemplateResponse>.Failure(FitnessProfileError.NotFound($"Fitness profile for user '{userId}' was not found"));
        
        var validationResult = await ValidateData(userId, request, ct);
        
        if(!validationResult.IsSuccess)
            return Result<CreateTemplateResponse>.Failure(validationResult.Errors.ToArray());
        
        var newTemplate = new WorkoutTemplate
        {
            Name = request.Name,
            Notes = request.Notes,
            OwnerId = userId,
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

    private async Task<Result> ValidateData(string userId, CreateTemplateRequest request, CancellationToken ct)
    {
        var numberOfTemplates = await context.WorkoutTemplates
            .Where(wt => wt.OwnerId == userId)
            .CountAsync(ct);
        
        if(numberOfTemplates == 20)
            return Result.Failure(WorkoutTemplateError.LimitReached("Maximum allowed number of workout templates per user is 20"));
        
        var normalizedTemplateName = request.Name.ToLower().Trim();

        var duplicateTemplate = await context.WorkoutTemplates
            .AnyAsync(wt => wt.OwnerId == userId 
                            && wt.Name.ToLower() == normalizedTemplateName, ct);
        
        if(duplicateTemplate)
            return Result.Failure(WorkoutTemplateError.AlreadyExists("Workout template with the same name already exists"));
        
        var invalidExerciseIds = await GetInvalidExerciseIds(request.Exercises.Select(e => e.ExerciseId).ToList(), ct);
        
        if(invalidExerciseIds.Count > 0)
            return Result.Failure(ExerciseError.NotFound("One or more requested exercises do not exist. Invalid exercise ids: " + string.Join(", ", invalidExerciseIds)));

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