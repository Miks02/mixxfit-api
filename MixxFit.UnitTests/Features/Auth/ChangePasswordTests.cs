using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Features.Auth.ChangePassword;
using MockQueryable;
using Moq;

namespace MixxFit.UnitTests.Features.Auth;

public class ChangePasswordTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly ChangePasswordHandler _handler;
    private readonly ChangePasswordRequest _request = new()
    {
        CurrentPassword = "current-password",
        NewPassword = "new-password",
        ConfirmPassword = "new-password"
    };

    public ChangePasswordTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new ChangePasswordHandler(_userManagerMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserIsNull_ShouldReturnFailure()
    {
        var emptyUserList = new List<User>().BuildMock();
        _userManagerMock.Setup(m => m.Users).Returns(emptyUserList);

        var result = await _handler.Handle("user-id-123", _request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().BeEquivalentTo(UserError.NotFound("user-id-123"));
        _tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("PasswordMismatch", "Incorrect password.")]
    [InlineData("PasswordTooShort", "Password is too short.")]
    [InlineData("PasswordRequiresNonAlphanumeric", "Passwords must have at least one non alphanumeric character.")]
    public async Task Handle_WhenChangePasswordFails_ShouldReturnFailure(string errorCode, string errorDescription)
    {
        var user = new User { Id = "user-id-123", UserName = "test-user", Email = "test-123@example.com" };
        var userList = new List<User> { user }.BuildMock();
        _userManagerMock.Setup(m => m.Users).Returns(userList);

        _userManagerMock
            .Setup(m => m.ChangePasswordAsync(user, _request.CurrentPassword, _request.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = errorCode, Description = errorDescription }));

        var result = await _handler.Handle("user-id-123", _request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == errorCode && e.Description == errorDescription);
        _tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenChangePasswordSucceeds_ShouldRevokeTokensAndReturnSuccess()
    {
        var user = new User { Id = "user-id-123", UserName = "test-user", Email = "test-123@example.com" };
        var userList = new List<User> { user }.BuildMock();
        _userManagerMock.Setup(m => m.Users).Returns(userList);

        _userManagerMock
            .Setup(m => m.ChangePasswordAsync(user, _request.CurrentPassword, _request.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle("user-id-123", _request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens("user-id-123"), Times.Once);
    }
}