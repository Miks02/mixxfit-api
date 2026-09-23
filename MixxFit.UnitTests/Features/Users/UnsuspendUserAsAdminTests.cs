using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Users.UnsuspendUserAsAdmin;
using MockQueryable;
using Moq;

namespace MixxFit.UnitTests.Features.Users;

public class UnsuspendUserAsAdminTests
{
    [Fact]
    public async Task Handle_WhenUserIsNull_ShouldReturnFailure()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        userManagerMock
            .Setup(m => m.Users)
            .Returns(new List<User>().BuildMock());

        var handler = new UnsuspendUserAsAdmin.UnsuspendUserAsAdminHandler(userManagerMock.Object);

        var result = await handler.Handle("missing-user-id");

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(UserError.NotFound("missing-user-id"));
    }

    [Fact]
    public async Task Handle_WhenUserIsAlreadyActive_ShouldReturnFailure()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var user = new User { Id = "user-id-123", AccountStatus = AccountStatus.Active };

        userManagerMock
            .Setup(m => m.Users)
            .Returns(new List<User> { user }.BuildMock());

        var handler = new UnsuspendUserAsAdmin.UnsuspendUserAsAdminHandler(userManagerMock.Object);

        var result = await handler.Handle("user-id-123");

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(UserError.UserAlreadyActive("user-id-123"));

        userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserIsSuspended_ShouldReactivateUser()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var user = new User { Id = "user-id-123", AccountStatus = AccountStatus.Suspended };

        userManagerMock
            .Setup(m => m.Users)
            .Returns(new List<User> { user }.BuildMock());

        userManagerMock
            .Setup(m => m.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var handler = new UnsuspendUserAsAdmin.UnsuspendUserAsAdminHandler(userManagerMock.Object);

        var result = await handler.Handle("user-id-123");

        result.IsSuccess.Should().BeTrue();
        user.AccountStatus.Should().Be(AccountStatus.Active);

        userManagerMock.Verify(m => m.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenIdentityUpdateFails_ShouldReturnFailure()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var user = new User { Id = "user-id-123", AccountStatus = AccountStatus.Suspended };

        userManagerMock
            .Setup(m => m.Users)
            .Returns(new List<User> { user }.BuildMock());

        userManagerMock
            .Setup(m => m.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "ConcurrencyFailure", Description = "boom" }));

        var handler = new UnsuspendUserAsAdmin.UnsuspendUserAsAdminHandler(userManagerMock.Object);

        var result = await handler.Handle("user-id-123");

        result.IsSuccess.Should().BeFalse();
    }
}
