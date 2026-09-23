using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Users.SuspendUserAsAdmin;
using MockQueryable;
using Moq;

namespace MixxFit.UnitTests.Features.Users;

public class SuspendUserAsAdminTests
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

        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new SuspendUserAsAdmin.SuspendUserAsAdminHandler(userManagerMock.Object, tokenServiceMock.Object);

        var result = await handler.Handle("missing-user-id");

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(UserError.NotFound("missing-user-id"));

        tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserIsAlreadySuspended_ShouldReturnFailure()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var user = new User { Id = "user-id-123", AccountStatus = AccountStatus.Suspended };

        userManagerMock
            .Setup(m => m.Users)
            .Returns(new List<User> { user }.BuildMock());

        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new SuspendUserAsAdmin.SuspendUserAsAdminHandler(userManagerMock.Object, tokenServiceMock.Object);

        var result = await handler.Handle("user-id-123");

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(UserError.UserAlreadySuspended("user-id-123"));

        userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
        tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserIsActive_ShouldSuspendUserAndRevokeTokens()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var user = new User { Id = "user-id-123", AccountStatus = AccountStatus.Active };

        userManagerMock
            .Setup(m => m.Users)
            .Returns(new List<User> { user }.BuildMock());

        userManagerMock
            .Setup(m => m.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new SuspendUserAsAdmin.SuspendUserAsAdminHandler(userManagerMock.Object, tokenServiceMock.Object);

        var result = await handler.Handle("user-id-123");

        result.IsSuccess.Should().BeTrue();
        user.AccountStatus.Should().Be(AccountStatus.Suspended);

        userManagerMock.Verify(m => m.UpdateAsync(user), Times.Once);
        tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens("user-id-123"), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenIdentityUpdateFails_ShouldReturnFailureAndNotRevokeTokens()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var user = new User { Id = "user-id-123", AccountStatus = AccountStatus.Active };

        userManagerMock
            .Setup(m => m.Users)
            .Returns(new List<User> { user }.BuildMock());

        userManagerMock
            .Setup(m => m.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "ConcurrencyFailure", Description = "boom" }));

        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new SuspendUserAsAdmin.SuspendUserAsAdminHandler(userManagerMock.Object, tokenServiceMock.Object);

        var result = await handler.Handle("user-id-123");

        result.IsSuccess.Should().BeFalse();
        tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens(It.IsAny<string>()), Times.Never);
    }
}
