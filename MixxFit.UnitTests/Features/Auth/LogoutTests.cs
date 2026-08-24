using Microsoft.Extensions.Logging;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Auth.Logout;
using Moq;

namespace MixxFit.UnitTests.Features.Auth;

public class LogoutTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_WhenRefreshTokenIsNullOrWhitespace_ShouldNotRevokeToken(string? invalidToken)
    {
        var mockTokenService = new Mock<ITokenService>();
        var handler = new LogoutHandler(mockTokenService.Object, Mock.Of<ILogger<LogoutHandler>>());

        await handler.Handle(invalidToken!);
        
        mockTokenService.Verify(service => service.RevokeRefreshToken(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task Handle_WithRefreshToken_ShouldRevokeToken()
    {
        var mockTokenService = new Mock<ITokenService>();
        var refreshToken = "valid-refresh-token";
        
        mockTokenService
            .Setup(s => s.RevokeRefreshToken(refreshToken))
            .ReturnsAsync(Result.Success());
        
        var handler = new LogoutHandler(mockTokenService.Object, Mock.Of<ILogger<LogoutHandler>>());
        
        await handler.Handle(refreshToken);
        
        mockTokenService.Verify(service => service.RevokeRefreshToken(refreshToken), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidToken_ShouldHandleFailure()
    {
        var mockTokenService = new Mock<ITokenService>();
        var handler = new LogoutHandler(mockTokenService.Object, Mock.Of<ILogger<LogoutHandler>>());
        var refreshToken = "invalid-refresh-token";

        mockTokenService
            .Setup(s => s.RevokeRefreshToken(refreshToken))
            .ReturnsAsync(Result.Failure(AuthError.ExpiredToken("Token does not exist")));  
        
        await handler.Handle(refreshToken);
        
        mockTokenService.Verify(s => s.RevokeRefreshToken(refreshToken), Times.Once);
    }
    
}