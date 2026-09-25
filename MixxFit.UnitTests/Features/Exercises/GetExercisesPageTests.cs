using AwesomeAssertions;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Exercises.GetExercisesPage;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Exercises.ExerciseTestData;

namespace MixxFit.UnitTests.Features.Exercises;

public class GetExercisesPageTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetExercisesPageHandler _handler;

    public GetExercisesPageTests()
    {
        SeedLookups(_context, UserId, OtherUserId);
        _context.Exercises.AddRange(
            Exercise(1, "Squat", ownerId: null, categoryId: BarbellCategoryId, muscleGroupId: LegsId),
            Exercise(2, "Bench Press", ownerId: null, categoryId: BarbellCategoryId, muscleGroupId: ChestId),
            Exercise(100, "Cable Row", categoryId: DumbbellCategoryId, muscleGroupId: BackId),
            Exercise(101, "Deleted Exercise", isDeleted: true),
            Exercise(200, "Their Exercise", OtherUserId));
        _context.SaveChanges();

        _handler = new GetExercisesPageHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    [Fact]
    public async Task Handle_WhenFitnessProfileDoesNotExist_ShouldReturnEmptyPage()
    {
        var result = await _handler.Handle("missing-user", CancellationToken.None);

        result.Exercises.Should().BeEmpty();
        result.MuscleGroups.Should().BeEmpty();
        result.ExerciseCategories.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSystemAndOwnExercisesOrderedByName()
    {
        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.Exercises.Select(e => e.Id).Should().Equal(2, 100, 1);
    }

    [Fact]
    public async Task Handle_ShouldProjectExercises()
    {
        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.Exercises.Single(e => e.Id == 100).Should().BeEquivalentTo(new
        {
            Id = 100,
            Name = "Cable Row (Dumbbell)",
            ExerciseCategoryName = "Dumbbell",
            MuscleGroupName = "Back",
            ExerciseType = ExerciseType.WeightLifting,
            IsUserDefined = true
        });
        result.Exercises.Single(e => e.Id == 2).IsUserDefined.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldReturnAllMuscleGroupsAndCategories()
    {
        var result = await _handler.Handle(UserId, CancellationToken.None);

        result.MuscleGroups.Should().BeEquivalentTo(MuscleGroups.Select(m => new MuscleGroupDto { Id = m.Id, Name = m.Name }));
        result.ExerciseCategories.Should().BeEquivalentTo(Categories.Select(c => new ExerciseCategoryDto { Id = c.Id, Name = c.Name }));
    }
}
