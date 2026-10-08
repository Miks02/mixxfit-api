using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.WorkoutTemplateExercises;
using MixxFit.API.Domain.Entities.WorkoutTemplates;
using MixxFit.API.Features.WorkoutTemplates.Common;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.WorkoutTemplates.EditTemplate;

public class EditTemplateHandler(AppDbContext context) : IHandler
{
    public async Task<Result<EditTemplateResponse>> Handle(string userId, EditTemplateRequest request, CancellationToken ct)
    {
        var templateToUpdate = await context.WorkoutTemplates
            .Include(wt => wt.WorkoutTemplateExercises)
            .Where(wt => wt.Id == request.Id && wt.OwnerId == userId)
            .FirstOrDefaultAsync(ct);
        
        if(templateToUpdate is null)
            return Result<EditTemplateResponse>.Failure(WorkoutTemplateError.NotFound($"Workout template with id '{request.Id}' was not found for user '{userId}'"));
        
        var validationResult = await ValidateData(userId, request, ct);
        
        if(!validationResult.IsSuccess)
            return Result<EditTemplateResponse>.Failure(validationResult.Errors.ToArray());
        
        templateToUpdate.Name = request.Name;
        templateToUpdate.Notes = request.Notes;
        
        var existingExercises = ToTemplateExerciseDto(templateToUpdate.WorkoutTemplateExercises);
        
        if(AreExercisesUpdateable(request.Exercises, existingExercises))
        {
            context.WorkoutTemplateExercises.RemoveRange(templateToUpdate.WorkoutTemplateExercises);
            templateToUpdate.WorkoutTemplateExercises = request.Exercises.Select((e, index) => new WorkoutTemplateExercise
            {
                ExerciseId = e.ExerciseId,
                SetCount = e.SetCount,
                Order = index + 1
            }).ToList();
        }
        
        await context.SaveChangesAsync(ct);

        var response = new EditTemplateResponse
        {
            Id = templateToUpdate.Id,
            Notes = templateToUpdate.Notes,
            Name = templateToUpdate.Name,
            Exercises = ToTemplateExerciseDto(templateToUpdate.WorkoutTemplateExercises)
        };
        
        return Result<EditTemplateResponse>.Success(response);
    }   
    
    private async Task<Result> ValidateData(string userId, EditTemplateRequest request, CancellationToken ct)
    {
        var normalizedTemplateName = request.Name.ToLower().Trim();

        var duplicateTemplate = await context.WorkoutTemplates
            .AnyAsync(wt => wt.OwnerId == userId
                            && wt.Id != request.Id
                            && wt.Name.ToLower() == normalizedTemplateName, ct);

        if(duplicateTemplate)
            return Result.Failure(WorkoutTemplateError.AlreadyExists("Workout template with the same name already exists"));

        var invalidExerciseIds = await GetInvalidExerciseIds(userId, request.Exercises.Select(e => e.ExerciseId).ToList(), ct);

        if(invalidExerciseIds.Count > 0)
            return Result.Failure(ExerciseError.NotFound("One or more requested exercises do not exist. Invalid exercise ids: " + string.Join(", ", invalidExerciseIds)));

        return Result.Success();
    }

    private async Task<IReadOnlyList<int>> GetInvalidExerciseIds(string userId, IReadOnlyList<int> exerciseIds, CancellationToken ct)
    {
        var validIds = await context.Exercises
            .Where(e => exerciseIds.Contains(e.Id) && (e.OwnerId == null || e.OwnerId == userId))
            .Select(e => e.Id)
            .ToListAsync(ct);

        return exerciseIds.Except(validIds).ToList();
    }
    
    private static bool AreExercisesUpdateable(
        IReadOnlyList<EditTemplateRequest.ExerciseItem> requestExercises, 
        IReadOnlyList<TemplateExerciseDto> existingExercises)
    {
        if (requestExercises.Count != existingExercises.Count)
            return true;
        
        var requestExercisesToDto = requestExercises.Select((e, index) => new TemplateExerciseDto
        {
            ExerciseId = e.ExerciseId,
            SetCount = e.SetCount,
            Order = index + 1
        }).ToList();
        
        return !requestExercisesToDto.SequenceEqual(existingExercises);
    }

    private static IReadOnlyList<TemplateExerciseDto> ToTemplateExerciseDto(IEnumerable<WorkoutTemplateExercise> exercises)
    {
        return exercises.Select(e => new TemplateExerciseDto
        {
            ExerciseId = e.ExerciseId,
            SetCount = e.SetCount,
            Order = e.Order
        }).OrderBy(e => e.Order).ToList();
    }
}