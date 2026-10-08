using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Exercises.CreateExercise;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Exercises.ExerciseTestData;

namespace MixxFit.UnitTests.Features.Exercises;

public class CreateExerciseTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly CreateExerciseHandler _handler;

    public CreateExerciseTests()
    {
        SeedLookups(_context, UserId, OtherUserId);
        _handler = new CreateExerciseHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private static CreateExerciseRequest Request(string name = "Paused Bench", int categoryId = BarbellCategoryId, int muscleGroupId = ChestId)
        => new() { Name = name, CategoryId = categoryId, MuscleGroupId = muscleGroupId };

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldPersistExerciseOwnedByUser()
    {
        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var saved = await _context.Exercises.SingleAsync();
        saved.Name.Should().Be("Paused Bench");
        saved.OwnerId.Should().Be(UserId);
        saved.ExerciseCategoryId.Should().Be(BarbellCategoryId);
        saved.MuscleGroupId.Should().Be(ChestId);
        saved.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldReturnMappedDto()
    {
        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        var dto = result.Payload!;
        dto.Id.Should().Be((await _context.Exercises.SingleAsync()).Id);
        dto.Name.Should().Be("Paused Bench (Barbell)");
        dto.ExerciseCategoryName.Should().Be("Barbell");
        dto.MuscleGroupName.Should().Be("Chest");
        dto.ExerciseType.Should().Be(ExerciseType.WeightLifting);
        dto.IsUserDefined.Should().BeTrue();
    }

    [Theory]
    [InlineData(CardioCategoryId, ExerciseType.Cardio)]
    [InlineData(DurationCategoryId, ExerciseType.Cardio)]
    [InlineData(BodyweightCategoryId, ExerciseType.BodyWeight)]
    [InlineData(AssistedBodyweightCategoryId, ExerciseType.BodyWeight)]
    [InlineData(OtherCategoryId, ExerciseType.Other)]
    [InlineData(StretchingCategoryId, ExerciseType.Stretching)]
    [InlineData(BarbellCategoryId, ExerciseType.WeightLifting)]
    [InlineData(DumbbellCategoryId, ExerciseType.WeightLifting)]
    public async Task Handle_ShouldDeriveExerciseTypeFromCategory(int categoryId, ExerciseType expected)
    {
        var result = await _handler.Handle(UserId, Request(categoryId: categoryId), CancellationToken.None);

        result.Payload!.ExerciseType.Should().Be(expected);
        (await _context.Exercises.SingleAsync()).ExerciseType.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_WhenUserAlreadyHasExerciseWithSameName_ShouldReturnAlreadyExists()
    {
        _context.Exercises.Add(Exercise(100, "Paused Bench"));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(ExerciseError.AlreadyExists());
        (await _context.Exercises.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task Handle_WhenAnotherUserHasExerciseWithSameName_ShouldSucceed()
    {
        _context.Exercises.Add(Exercise(100, "Paused Bench", OtherUserId));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenSystemExerciseHasSameName_ShouldSucceed()
    {
        _context.Exercises.Add(Exercise(100, "Paused Bench", ownerId: null));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenExerciseWithSameNameIsDeleted_ShouldSucceed()
    {
        _context.Exercises.Add(Exercise(100, "Paused Bench", isDeleted: true));
        await _context.SaveChangesAsync();

        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenMuscleGroupDoesNotExist_ShouldReturnMuscleGroupNotFound()
    {
        var result = await _handler.Handle(UserId, Request(muscleGroupId: 999), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(ExerciseError.MuscleGroupNotFound(999));
        (await _context.Exercises.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ShouldReturnCategoryNotFound()
    {
        var result = await _handler.Handle(UserId, Request(categoryId: 999), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(ExerciseError.ExerciseCategoryNotFound(999));
        (await _context.Exercises.AnyAsync()).Should().BeFalse();
    }
}
