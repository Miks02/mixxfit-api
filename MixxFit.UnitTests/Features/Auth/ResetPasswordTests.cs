using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Auth.ResetPassword;
using Moq;

namespace MixxFit.UnitTests.Features.Auth;

public class ResetPasswordTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly ResetPasswordHandler _handler;
    private readonly ResetPasswordRequest _request = new()
    {
        UserId = "user-id-123",
        Token = "reset-token",
        Password = "NewPassword123!",
        ConfirmedPassword = "NewPassword123!"
    };

    public ResetPasswordTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new ResetPasswordHandler(_userManagerMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserIsNull_ShouldReturnInvalidPasswordResetToken()
    {
        _userManagerMock
            .Setup(m => m.FindByIdAsync(_request.UserId))
            .ReturnsAsync((User?)null);

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(AuthError.InvalidPasswordResetToken().Code);
        _userManagerMock.Verify(
            m => m.ResetPasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("InvalidToken", "Invalid token.")]
    [InlineData("PasswordTooShort", "Password is too short.")]
    public async Task Handle_WhenResetPasswordFails_ShouldReturnInvalidPasswordResetToken(string errorCode, string errorDescription)
    {
        var user = new User { Id = "user-id-123", Email = "test@example.com" };
        _userManagerMock.Setup(m => m.FindByIdAsync(_request.UserId)).ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.ResetPasswordAsync(user, _request.Token, _request.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = errorCode, Description = errorDescription }));

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Code.Should().Be(AuthError.InvalidPasswordResetToken().Code);
        _tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenResetPasswordSucceeds_ShouldRevokeTokensAndReturnSuccess()
    {
        var user = new User { Id = "user-id-123", Email = "test@example.com" };
        _userManagerMock.Setup(m => m.FindByIdAsync(_request.UserId)).ReturnsAsync(user);
        _userManagerMock
            .Setup(m => m.ResetPasswordAsync(user, _request.Token, _request.Password))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _userManagerMock.Verify(m => m.ResetPasswordAsync(user, _request.Token, _request.Password), Times.Once);
        _tokenServiceMock.Verify(t => t.RevokeAllRefreshTokens("user-id-123"), Times.Once);
    }
}
