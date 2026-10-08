using AwesomeAssertions;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Exercises.GetExercises;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.UnitTests.TestUtilities;
using static MixxFit.UnitTests.Features.Exercises.ExerciseTestData;

namespace MixxFit.UnitTests.Features.Exercises;

public class GetExercisesTests : IDisposable
{
    private readonly AppDbContext _context = InMemoryTestDatabase.CreateContext();
    private readonly GetExercisesHandler _handler;

    public GetExercisesTests()
    {
        SeedLookups(_context, UserId, OtherUserId);
        _context.Exercises.AddRange(
            Exercise(1, "Bench Press", ownerId: null, categoryId: BarbellCategoryId, muscleGroupId: ChestId),
            Exercise(2, "Running", ownerId: null, categoryId: CardioCategoryId, muscleGroupId: LegsId, type: ExerciseType.Cardio),
            Exercise(3, "Dumbbell Row", ownerId: null, categoryId: DumbbellCategoryId, muscleGroupId: BackId),
            Exercise(100, "My Bench Variation", categoryId: BarbellCategoryId, muscleGroupId: ChestId),
            Exercise(101, "My Sprint", categoryId: CardioCategoryId, muscleGroupId: LegsId, type: ExerciseType.Cardio),
            Exercise(102, "My Deleted", isDeleted: true),
            Exercise(200, "Their Bench", OtherUserId, categoryId: BarbellCategoryId));
        _context.SaveChanges();

        _handler = new GetExercisesHandler(_context);
    }

    public void Dispose() => _context.Dispose();

    private Task<GetExercisesResponse> Handle(GetExercisesRequest request) =>
        _handler.Handle(UserId, request, CancellationToken.None);

    [Fact]
    public async Task Handle_WhenOnlyUserDefinedIsFalse_ShouldReturnSystemAndOwnExercises()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = false });

        result.Exercises.Select(e => e.Id).Should().BeEquivalentTo([1, 2, 3, 100, 101]);
    }

    [Fact]
    public async Task Handle_WhenOnlyUserDefinedIsNotProvided_ShouldReturnSystemAndOwnExercises()
    {
        var result = await Handle(new GetExercisesRequest());

        result.Exercises.Select(e => e.Id).Should().BeEquivalentTo([1, 2, 3, 100, 101]);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_ShouldNeverReturnOtherUsersExercises(bool? onlyUserDefined)
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = onlyUserDefined, SearchTerm = "Bench" });

        result.Exercises.Should().NotContain(e => e.Id == 200);
    }

    [Fact]
    public async Task Handle_WhenOnlyUserDefinedIsTrue_ShouldReturnOnlyOwnExercises()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = true });

        result.Exercises.Select(e => e.Id).Should().BeEquivalentTo([100, 101]);
        result.Exercises.Should().OnlyContain(e => e.IsUserDefined);
    }

    [Fact]
    public async Task Handle_ShouldNotReturnDeletedExercises()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = true });

        result.Exercises.Should().NotContain(e => e.Id == 102);
    }

    [Fact]
    public async Task Handle_ShouldProjectNameWithCategoryAndMuscleGroup()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = false });

        var own = result.Exercises.Single(e => e.Id == 101);
        own.Name.Should().Be("My Sprint (Cardio)");
        own.MuscleGroupName.Should().Be("Legs");
        own.ExerciseCategoryName.Should().Be("Cardio");
        own.ExerciseType.Should().Be(ExerciseType.Cardio);
        own.IsUserDefined.Should().BeTrue();

        var system = result.Exercises.Single(e => e.Id == 1);
        system.Name.Should().Be("Bench Press (Barbell)");
        system.ExerciseCategoryName.Should().Be("Barbell");
        system.IsUserDefined.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldOrderByCategory()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = false });

        result.Exercises.Select(e => e.Name).Should().ContainInOrder(
            "Running (Cardio)", "Bench Press (Barbell)", "Dumbbell Row (Dumbbell)");
    }

    [Fact]
    public async Task Handle_WhenFilteringByCategory_ShouldReturnMatchingExercises()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = false, CategoryId = BarbellCategoryId });

        result.Exercises.Select(e => e.Id).Should().BeEquivalentTo([1, 100]);
    }

    [Fact]
    public async Task Handle_WhenFilteringByMuscleGroup_ShouldReturnMatchingExercises()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = false, MuscleGroupId = LegsId });

        result.Exercises.Select(e => e.Id).Should().BeEquivalentTo([2, 101]);
    }

    [Fact]
    public async Task Handle_WhenCombiningFilters_ShouldApplyAllOfThem()
    {
        var result = await Handle(new GetExercisesRequest
        {
            OnlyUserDefined = true,
            CategoryId = CardioCategoryId,
            MuscleGroupId = LegsId,
            SearchTerm = "Sprint"
        });

        result.Exercises.Select(e => e.Id).Should().Equal(101);
    }

    [Fact]
    public async Task Handle_WhenSearching_ShouldFilterByName()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = false, SearchTerm = "Bench" });

        result.Exercises.Select(e => e.Id).Should().BeEquivalentTo([1, 100]);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_WhenSearchTermIsBlank_ShouldIgnoreIt(string searchTerm)
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = false, SearchTerm = searchTerm });

        result.Exercises.Should().HaveCount(5);
    }

    [Fact]
    public async Task Handle_WhenNothingMatches_ShouldReturnEmptyList()
    {
        var result = await Handle(new GetExercisesRequest { OnlyUserDefined = false, SearchTerm = "Nonexistent" });

        result.Exercises.Should().BeEmpty();
    }
}
