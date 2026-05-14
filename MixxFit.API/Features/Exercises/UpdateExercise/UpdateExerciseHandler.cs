using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Exercises.Shared;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;

namespace MixxFit.API.Features.Exercises.UpdateExercise;

public class UpdateExerciseHandler(AppDbContext context) : IHandler
{
    public async Task<Result<ExerciseDto>> Handle(
        string userId, 
        UpdateExerciseRequest request, 
        CancellationToken cancellationToken)
    {
        var fitnessProfile = await context.FitnessProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(fp => fp.UserId == userId, cancellationToken);

        if (fitnessProfile is null)
        {
            return Result<ExerciseDto>.Failure(FitnessProfileError.NotFound());
        }
        
        var exerciseToUpdate = await context.Exercises
            .FirstOrDefaultAsync(e => e.Id == request.Id && e.FitnessProfileId == fitnessProfile.Id, cancellationToken);
        
        if (exerciseToUpdate is null)
            return Result<ExerciseDto>.Failure(ExerciseError.NotFound($"Exercise with id: {request.Id} has not been found"));
        
        var exerciseExists = await context.Exercises
            .Where(e => e.Name == request.Name && e.FitnessProfileId == fitnessProfile.Id && e.Id != request.Id)
            .AnyAsync(cancellationToken);
        
        if(exerciseExists)
            return Result<ExerciseDto>.Failure(ExerciseError.AlreadyExists());
        
        var muscleGroupName = await context.MuscleGroups
            .Where(m => m.Id == request.MuscleGroupId)
            .Select(e => e.Name)
            .FirstOrDefaultAsync(cancellationToken);

        if(muscleGroupName is null)
            return Result<ExerciseDto>.Failure(ExerciseError.MuscleGroupNotFound(request.MuscleGroupId));

        var exerciseCategoryName = await context.ExerciseCategories
            .Where(e => e.Id == request.CategoryId)
            .Select(e => e.Name)
            .FirstOrDefaultAsync(cancellationToken);

        if(exerciseCategoryName is null)
            return Result<ExerciseDto>.Failure(ExerciseError.ExerciseCategoryNotFound(request.CategoryId));
        
        exerciseToUpdate.Name = request.Name;
        exerciseToUpdate.MuscleGroupId = request.MuscleGroupId;
        exerciseToUpdate.ExerciseCategoryId = request.CategoryId;
        exerciseToUpdate.ExerciseType = await GetExerciseType(request.CategoryId);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return Result<ExerciseDto>.Success(new ExerciseDto
        {
            Id = exerciseToUpdate.Id,
            Name = exerciseToUpdate.Name + $" ({exerciseCategoryName})",
            MuscleGroupName = muscleGroupName,
            ExerciseCategoryName = exerciseCategoryName,
            ExerciseType = exerciseToUpdate.ExerciseType,
            IsUserDefined = true
        });
    }
    
    private async Task<ExerciseType> GetExerciseType(int categoryId)
    {
        var category = await context.ExerciseCategories
            .Where(c => c.Id == categoryId)
            .Select(c => c.Name)
            .FirstOrDefaultAsync();

        if(category is null)
            throw new InvalidOperationException("Category not found");

        return category switch
        {
            "Cardio" or "Duration" => ExerciseType.Cardio,
            "Bodyweight" or "Assisted Bodyweight" => ExerciseType.BodyWeight,
            "Other" => ExerciseType.Other,
            "Stretching" => ExerciseType.Stretching,
            _ => ExerciseType.WeightLifting
        };
    }
}