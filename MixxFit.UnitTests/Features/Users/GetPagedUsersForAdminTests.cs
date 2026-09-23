using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Features.Users.GetPagedUsersForAdmin;
using MixxFit.API.Infrastructure.Persistence;
using MockQueryable.Moq;
using Moq;

namespace MixxFit.UnitTests.Features.Users;

public class GetPagedUsersForAdminTests
{
    private static User CreateUser(
        string id,
        string firstName,
        string lastName,
        string email,
        DateTime createdAt,
        DateTime? deletedAt = null,
        DateTime? dateOfBirth = null,
        int workouts = 0,
        int weightEntries = 0) => new()
    {
        Id = id,
        FirstName = firstName,
        LastName = lastName,
        UserName = id,
        Email = email,
        CreatedAt = createdAt,
        DeletedAt = deletedAt,
        FitnessProfile = new FitnessProfile
        {
            UserId = id,
            DateOfBirth = dateOfBirth,
            Workouts = Enumerable.Range(0, workouts).Select(i => new Workout { Id = i, OwnerId = id }).ToList(),
            WeightEntries = Enumerable.Range(0, weightEntries).Select(i => new WeightEntry { Id = i, OwnerId = id }).ToList()
        }
    };

    private static GetPagedUsersForAdminHandler CreateHandler(params User[] users)
    {
        var contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
        contextMock.Setup(c => c.Users).Returns(users.ToList().BuildMockDbSet().Object);
        return new GetPagedUsersForAdminHandler(contextMock.Object);
    }

    private static readonly User[] Users =
    [
        CreateUser("1", "Ana", "Zaric", "ana@mail.com", new DateTime(2026, 1, 1), workouts: 3, weightEntries: 2,
            dateOfBirth: new DateTime(1990, 1, 1)),
        CreateUser("2", "Bojan", "Milic", "bojan@mail.com", new DateTime(2026, 2, 1)),
        CreateUser("3", "Deleted", "Deleted", "deleted_3@deleted.invalid", new DateTime(2026, 3, 1),
            deletedAt: new DateTime(2026, 4, 1), workouts: 1)
    ];

    [Fact]
    public async Task Handle_WithDefaults_ShouldReturnAllUsersNewestFirst()
    {
        var result = await CreateHandler(Users).Handle(new GetPagedUsersForAdminRequest());

        result.TotalCount.Should().Be(3);
        result.PaginatedCount.Should().Be(3);
        result.Items.Select(u => u.Id).Should().Equal("3", "2", "1");
    }

    [Fact]
    public async Task Handle_ShouldProjectCountsAndAge()
    {
        var result = await CreateHandler(Users).Handle(new GetPagedUsersForAdminRequest { Search = "ana@" });

        var user = result.Items.Should().ContainSingle().Subject;
        user.WorkoutCount.Should().Be(3);
        user.WeightEntryCount.Should().Be(2);
        user.Age.Should().BeGreaterThan(30);
        user.Email.Should().Be("ana@mail.com");
    }

    [Theory]
    [InlineData(true, new[] { "3" })]
    [InlineData(false, new[] { "2", "1" })]
    public async Task Handle_WhenFilteringByIsDeleted_ShouldReturnMatchingUsers(bool isDeleted, string[] expectedIds)
    {
        var result = await CreateHandler(Users).Handle(new GetPagedUsersForAdminRequest { IsDeleted = isDeleted });

        result.Items.Select(u => u.Id).Should().Equal(expectedIds);
        result.TotalCount.Should().Be(expectedIds.Length);
    }

    [Theory]
    [InlineData("BOJAN", "2")]
    [InlineData("milic", "2")]
    [InlineData("ana zaric", "1")]
    [InlineData("mail.com", "1,2")]
    public async Task Handle_WhenSearching_ShouldMatchCaseInsensitivelyOnNameAndEmail(string search, string expected)
    {
        var result = await CreateHandler(Users).Handle(new GetPagedUsersForAdminRequest { Search = search, Sort = "oldest" });

        result.Items.Select(u => u.Id).Should().Equal(expected.Split(','));
    }

    [Theory]
    [InlineData("oldest", new[] { "1", "2", "3" })]
    [InlineData("name", new[] { "3", "2", "1" })]
    [InlineData("email", new[] { "1", "2", "3" })]
    [InlineData("newest", new[] { "3", "2", "1" })]
    public async Task Handle_WhenSorting_ShouldOrderUsers(string sort, string[] expectedIds)
    {
        var result = await CreateHandler(Users).Handle(new GetPagedUsersForAdminRequest { Sort = sort });

        result.Items.Select(u => u.Id).Should().Equal(expectedIds);
    }

    [Fact]
    public async Task Handle_WhenPaginating_ShouldReturnRequestedPageWithTotalCount()
    {
        var result = await CreateHandler(Users).Handle(new GetPagedUsersForAdminRequest { Page = 2, PageSize = 2, Sort = "oldest" });

        result.Items.Select(u => u.Id).Should().Equal("3");
        result.TotalCount.Should().Be(3);
        result.PaginatedCount.Should().Be(1);
        result.TotalPages.Should().Be(2);
    }
}
