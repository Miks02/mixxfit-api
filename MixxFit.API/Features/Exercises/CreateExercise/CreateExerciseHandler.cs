using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Exercises.CreateExercise;

public class CreateExerciseHandler(AppDbContext context) : IHandler
{
    public async Task<Result<CreateExerciseResponse>> Handle(
        string userId, 
        CreateExerciseRequest request, 
        CancellationToken cancellationToken)
    {
        var exerciseExists = await context.Exercises
            .Where(e => e.Name == request.Name && e.UserId == userId)
            .AnyAsync(cancellationToken);
        
        if(exerciseExists)
            return Result<CreateExerciseResponse>.Failure(GeneralError.Conflict("Exercise already exists"));
        
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
        
        return Result<CreateExerciseResponse>.Success(new CreateExerciseResponse
        {
            Id = newExercise.Id,
            Name = request.Name
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