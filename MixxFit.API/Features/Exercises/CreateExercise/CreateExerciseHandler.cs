using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Exercises.Shared;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Domain.Entities.Exercises;

namespace MixxFit.API.Features.Exercises.CreateExercise;

public class CreateExerciseHandler(AppDbContext context) : IHandler
{
    public async Task<Result<ExerciseDto>> Handle(
        string userId,
        CreateExerciseRequest request,
        CancellationToken cancellationToken)
    {
        var exerciseExists = await context.Exercises
            .Where(e => e.Name == request.Name && e.UserId == userId)
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

        var newExercise = new Exercise
        {
            Name = request.Name,
            ExerciseCategoryId = request.CategoryId,
            MuscleGroupId = request.MuscleGroupId,
            UserId = userId,
            ExerciseType = await GetExerciseType(request.CategoryId)
        };

        context.Add(newExercise);
        await context.SaveChangesAsync(cancellationToken);

        return Result<ExerciseDto>.Success(new ExerciseDto
        {
            Id = newExercise.Id,
            Name = $"{newExercise.Name} ({exerciseCategoryName})",
            MuscleGroupName = muscleGroupName,
            ExerciseCategoryName = exerciseCategoryName,
            ExerciseType = newExercise.ExerciseType,
            IsUserDefined = true,
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
