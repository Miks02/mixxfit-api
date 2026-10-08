using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Exercises.UpdateExercise;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Exercises.ExerciseTestData;

namespace MixxFit.UnitTests.Features.Exercises;

public class UpdateExerciseTests : IDisposable
{
    private const int ExerciseId = 100;

    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly UpdateExerciseHandler _handler;

    public UpdateExerciseTests()
    {
        SeedLookups(_context, UserId, OtherUserId);
        _context.Exercises.AddRange(
            Exercise(ExerciseId, "Paused Bench"),
            Exercise(101, "Spoto Press"),
            Exercise(200, "Their Exercise", OtherUserId),
            Exercise(300, "System Exercise", ownerId: null),
            Exercise(400, "Deleted Exercise", isDeleted: true));
        _context.SaveChanges();
        _context.ChangeTracker.Clear();

        _handler = new UpdateExerciseHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private static UpdateExerciseRequest Request(
        int id = ExerciseId,
        string name = "Tempo Squat",
        int categoryId = BodyweightCategoryId,
        int muscleGroupId = LegsId)
        => new() { Id = id, Name = name, CategoryId = categoryId, MuscleGroupId = muscleGroupId };

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldUpdateExercise()
    {
        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var saved = await _context.Exercises.AsNoTracking().SingleAsync(e => e.Id == ExerciseId);
        saved.Name.Should().Be("Tempo Squat");
        saved.ExerciseCategoryId.Should().Be(BodyweightCategoryId);
        saved.MuscleGroupId.Should().Be(LegsId);
        saved.ExerciseType.Should().Be(ExerciseType.BodyWeight);
        saved.OwnerId.Should().Be(UserId);
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldReturnMappedDto()
    {
        var result = await _handler.Handle(UserId, Request(), CancellationToken.None);

        result.Payload.Should().BeEquivalentTo(new
        {
            Id = ExerciseId,
            Name = "Tempo Squat (Bodyweight)",
            MuscleGroupName = "Legs",
            ExerciseCategoryName = "Bodyweight",
            ExerciseType = ExerciseType.BodyWeight,
            IsUserDefined = true
        });
    }

    [Theory]
    [InlineData(CardioCategoryId, ExerciseType.Cardio)]
    [InlineData(DurationCategoryId, ExerciseType.Cardio)]
    [InlineData(AssistedBodyweightCategoryId, ExerciseType.BodyWeight)]
    [InlineData(StretchingCategoryId, ExerciseType.Stretching)]
    [InlineData(OtherCategoryId, ExerciseType.Other)]
    [InlineData(DumbbellCategoryId, ExerciseType.WeightLifting)]
    public async Task Handle_ShouldDeriveExerciseTypeFromCategory(int categoryId, ExerciseType expected)
    {
        var result = await _handler.Handle(UserId, Request(categoryId: categoryId), CancellationToken.None);

        result.Payload!.ExerciseType.Should().Be(expected);
    }

    [Fact]
    public async Task Handle_WhenKeepingSameName_ShouldNotReportDuplicate()
    {
        var result = await _handler.Handle(UserId, Request(name: "Paused Bench"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenFitnessProfileDoesNotExist_ShouldReturnFitnessProfileNotFound()
    {
        var result = await _handler.Handle("missing-user", Request(), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(FitnessProfileError.NotFound());
    }

    [Theory]
    [InlineData(999)]
    [InlineData(200)]
    [InlineData(300)]
    [InlineData(400)]
    public async Task Handle_WhenExerciseIsMissingNotOwnedOrDeleted_ShouldReturnNotFound(int id)
    {
        var result = await _handler.Handle(UserId, Request(id: id), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(ExerciseError.NotFound().Code);
    }

    [Fact]
    public async Task Handle_WhenExerciseBelongsToAnotherUser_ShouldNotModifyIt()
    {
        await _handler.Handle(UserId, Request(id: 200), CancellationToken.None);

        (await _context.Exercises.AsNoTracking().SingleAsync(e => e.Id == 200)).Name.Should().Be("Their Exercise");
    }

    [Fact]
    public async Task Handle_WhenAnotherOwnExerciseHasSameName_ShouldReturnAlreadyExists()
    {
        var result = await _handler.Handle(UserId, Request(name: "Spoto Press"), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(ExerciseError.AlreadyExists());
        (await _context.Exercises.AsNoTracking().SingleAsync(e => e.Id == ExerciseId)).Name.Should().Be("Paused Bench");
    }

    [Fact]
    public async Task Handle_WhenOtherUserOrSystemExerciseHasSameName_ShouldSucceed()
    {
        var first = await _handler.Handle(UserId, Request(name: "Their Exercise"), CancellationToken.None);
        var second = await _handler.Handle(UserId, Request(name: "System Exercise"), CancellationToken.None);

        first.IsSuccess.Should().BeTrue();
        second.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenMuscleGroupDoesNotExist_ShouldReturnMuscleGroupNotFound()
    {
        var result = await _handler.Handle(UserId, Request(muscleGroupId: 999), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(ExerciseError.MuscleGroupNotFound(999));
        (await _context.Exercises.AsNoTracking().SingleAsync(e => e.Id == ExerciseId)).Name.Should().Be("Paused Bench");
    }

    [Fact]
    public async Task Handle_WhenCategoryDoesNotExist_ShouldReturnCategoryNotFound()
    {
        var result = await _handler.Handle(UserId, Request(categoryId: 999), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(ExerciseError.ExerciseCategoryNotFound(999));
    }
}
