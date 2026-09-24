using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Entities.WeightEntries;
using MixxFit.API.Domain.Entities.Workouts;
using MixxFit.API.Domain.Entities.Exercises;
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
    private readonly Mock<IAuthEmailSender> _emailSenderMock = new();

    public DeleteUserAsAdminTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());

        _userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string>());

        _contextMock.Setup(c => c.Workouts).Returns(new List<Workout>().BuildMockDbSet().Object);
        _contextMock.Setup(c => c.WeightEntries).Returns(new List<WeightEntry>().BuildMockDbSet().Object);
        _contextMock.Setup(c => c.Exercises).Returns(new List<Exercise>().BuildMockDbSet().Object);
    }

    private DeleteUserAsAdmin.DeleteUserAsAdminHandler CreateHandler(User? user)
    {
        _userManagerMock
            .Setup(m => m.Users)
            .Returns((user is null ? new List<User>() : new List<User> { user }).BuildMock());

        var deleteUserHandler = new DeleteUserHandler(
            _userManagerMock.Object,
            _contextMock.Object,
            new Mock<ITokenService>().Object,
            new Mock<IFileService>().Object,
            _emailSenderMock.Object);

        return new DeleteUserAsAdmin.DeleteUserAsAdminHandler(deleteUserHandler);
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
    public async Task Handle_WhenUserExists_ShouldDeleteUserAndSendDeletedEmail()
    {
        var user = new User
        {
            Id = UserId,
            Email = "john@doe.com",
            FitnessProfile = new FitnessProfile { UserId = UserId }
        };
        var handler = CreateHandler(user);

        var result = await handler.Handle(UserId);

        result.IsSuccess.Should().BeTrue();
        _userManagerMock.Verify(m => m.DeleteAsync(user), Times.Once);
        _emailSenderMock.Verify(e => e.SendAccountDeletedEmailAsync("john@doe.com"), Times.Once);
    }
}
