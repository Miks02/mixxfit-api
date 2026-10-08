using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Features.Auth.SendResetPasswordLink;
using Moq;

namespace MixxFit.UnitTests.Features.Auth;

public class SendResetPasswordLinkTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IAuthEmailSender> _authEmailSenderMock;
    private readonly SendResetPasswordLinkHandler _handler;
    private readonly SendResetPasswordLinkRequest _request = new("test@example.com");

    public SendResetPasswordLinkTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );
        _authEmailSenderMock = new Mock<IAuthEmailSender>();
        _handler = new SendResetPasswordLinkHandler(_userManagerMock.Object, _authEmailSenderMock.Object);
    }

    [Fact]
    public async Task Handle_WhenUserIsNull_ShouldReturnSuccessAndNotSendEmail()
    {
        _userManagerMock
            .Setup(m => m.FindByEmailAsync(_request.Email))
            .ReturnsAsync((User?)null);

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeTrue();
        _userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Never);
        _authEmailSenderMock.Verify(
            e => e.SendPasswordResetEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(AccountStatus.Suspended)]
    [InlineData(AccountStatus.Deleted)]
    public async Task Handle_WhenUserIsNotActive_ShouldReturnSuccessAndNotSendEmail(AccountStatus status)
    {
        var user = new User { Id = "user-id-123", Email = _request.Email, AccountStatus = status };
        _userManagerMock.Setup(m => m.FindByEmailAsync(_request.Email)).ReturnsAsync(user);

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeTrue();
        _userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsync(It.IsAny<User>()), Times.Never);
        _authEmailSenderMock.Verify(
            e => e.SendPasswordResetEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenUserIsActive_ShouldGenerateTokenAndSendEmail()
    {
        var user = new User { Id = "user-id-123", Email = _request.Email, AccountStatus = AccountStatus.Active };
        _userManagerMock.Setup(m => m.FindByEmailAsync(_request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("reset-token");

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeTrue();
        _userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsync(user), Times.Once);
        _authEmailSenderMock.Verify(
            e => e.SendPasswordResetEmailAsync("test@example.com", "user-id-123", "reset-token"), Times.Once);
    }
}
