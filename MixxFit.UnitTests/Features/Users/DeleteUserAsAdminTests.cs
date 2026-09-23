using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.Exercises;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Users.DeleteUser;
using MixxFit.API.Features.Users.DeleteUserAsAdmin;
using MixxFit.API.Infrastructure.Persistence;
using MockQueryable;
using MockQueryable.Moq;
using Moq;

namespace MixxFit.UnitTests.Features.Users;

public class DeleteUserAsAdminTests
{
    private const string UserId = "3f2a9c1e-7b4d-4e8a-9d21-5c6b0a1f2e34";

    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<AppDbContext> _contextMock;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<IFileService> _fileServiceMock = new();

    public DeleteUserAsAdminTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());

        _fileServiceMock
            .Setup(f => f.DeleteFile(It.IsAny<string>()))
            .ReturnsAsync(Result.Success());

        _userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "User" });
        _userManagerMock
            .Setup(m => m.RemoveFromRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    private static User CreateUser() => new()
    {
        Id = UserId,
        FirstName = "John",
        LastName = "Doe",
        UserName = "johndoe",
        Email = "john@doe.com",
        PhoneNumber = "123456",
        ImagePath = "images/john.png",
        PasswordHash = "hash",
        FitnessProfile = new FitnessProfile
        {
            UserId = UserId,
            Height = 180,
            Weight = 80,
            TargetWeight = 75,
            DailyCalorieGoal = 2500,
            DateOfBirth = new DateTime(1995, 1, 1),
            Gender = Gender.Male
        }
    };

    private DeleteUserAsAdmin.DeleteUserAsAdminHandler CreateHandler(
        User? user,
        List<Workout>? workouts = null,
        List<WeightEntry>? weights = null,
        List<Exercise>? exercises = null)
    {
        _userManagerMock
            .Setup(m => m.Users)
            .Returns((user is null ? new List<User>() : new List<User> { user }).BuildMock());

        _contextMock.Setup(c => c.Workouts).Returns((workouts ?? []).BuildMockDbSet().Object);
        _contextMock.Setup(c => c.WeightEntries).Returns((weights ?? []).BuildMockDbSet().Object);
        _contextMock.Setup(c => c.Exercises).Returns((exercises ?? []).BuildMockDbSet().Object);

        var deleteUserHandler = new DeleteUserHandler(_userManagerMock.Object, _fileServiceMock.Object);

        return new DeleteUserAsAdmin.DeleteUserAsAdminHandler(
            _userManagerMock.Object,
            _contextMock.Object,
            _tokenServiceMock.Object,
            _fileServiceMock.Object,
            deleteUserHandler);
    }

    [Fact]
    public async Task Handle_WhenUserIsNull_ShouldReturnFailure()
    {
        var handler = CreateHandler(null);

        var result = await handler.Handle(UserId);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(UserError.NotFound(UserId));
    }

    [Fact]
    public async Task Handle_WhenUserIsAlreadyDeleted_ShouldReturnFailure()
    {
        var user = CreateUser();
        user.DeletedAt = DateTime.UtcNow;
        var handler = CreateHandler(user);

        var result = await handler.Handle(UserId);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(UserError.UserAlreadyDeleted(UserId));
        _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
        _userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserHasNoRelevantData_ShouldHardDelete()
    {
        var user = CreateUser();
        var handler = CreateHandler(user);

        var result = await handler.Handle(UserId);

        result.IsSuccess.Should().BeTrue();
        _userManagerMock.Verify(m => m.DeleteAsync(user), Times.Once);
        _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
        user.DeletedAt.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenUserHasWorkout_ShouldAnonymizeInsteadOfDelete()
    {
        var user = CreateUser();
        var handler = CreateHandler(user, workouts: [new Workout { Id = 1, Name = "Push", OwnerId = UserId }]);

        var result = await handler.Handle(UserId);

        AssertAnonymized(result, user);
    }

    [Fact]
    public async Task Handle_WhenUserHasWeightEntry_ShouldAnonymizeInsteadOfDelete()
    {
        var user = CreateUser();
        var handler = CreateHandler(user, weights: [new WeightEntry { Id = 1, OwnerId = UserId, Weight = 80 }]);

        var result = await handler.Handle(UserId);

        AssertAnonymized(result, user);
    }

    [Fact]
    public async Task Handle_WhenUserHasCustomExercise_ShouldAnonymizeInsteadOfDelete()
    {
        var user = CreateUser();
        var handler = CreateHandler(user, exercises: [new Exercise { Id = 1, Name = "Mine", OwnerId = UserId }]);

        var result = await handler.Handle(UserId);

        AssertAnonymized(result, user);
    }

    [Fact]
    public async Task Handle_WhenAnonymizing_ShouldDeleteImageAndRevokeTokensAndRemoveRoles()
    {
        var user = CreateUser();
        var handler = CreateHandler(user, workouts: [new Workout { Id = 1, Name = "Push", OwnerId = UserId }]);

        await handler.Handle(UserId);

        _fileServiceMock.Verify(f => f.DeleteFile("images/john.png"), Times.Once);
        _tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens(UserId), Times.Once);
        _userManagerMock.Verify(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenImageDeletionFails_ShouldReturnFailureAndNotUpdateUser()
    {
        _fileServiceMock
            .Setup(f => f.DeleteFile(It.IsAny<string>()))
            .ReturnsAsync(Result.Failure(new Error("File.DeleteFailed", "boom")));
        var user = CreateUser();
        var handler = CreateHandler(user, workouts: [new Workout { Id = 1, Name = "Push", OwnerId = UserId }]);

        var result = await handler.Handle(UserId);

        result.IsSuccess.Should().BeFalse();
        _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
        user.DeletedAt.Should().BeNull();
    }

    private void AssertAnonymized(Result result, User user)
    {
        result.IsSuccess.Should().BeTrue();
        _userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<User>()), Times.Never);
        _userManagerMock.Verify(m => m.UpdateAsync(user), Times.Once);

        user.Id.Should().Be(UserId);
        user.FirstName.Should().Be("Deleted");
        user.LastName.Should().Be("Deleted");
        user.UserName.Should().StartWith("deleted_").And.HaveLength(20);
        user.Email.Should().EndWith("@deleted.invalid");
        user.PhoneNumber.Should().BeNull();
        user.ImagePath.Should().BeNull();
        user.PasswordHash.Should().BeNull();
        user.AccountStatus.Should().Be(AccountStatus.Deleted);
        user.DeletedAt.Should().NotBeNull();
        user.FitnessProfile.DateOfBirth.Should().BeNull();
        user.FitnessProfile.Height.Should().BeNull();
        user.FitnessProfile.Weight.Should().BeNull();
    }
}
