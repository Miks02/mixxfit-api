using AwesomeAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.RefreshTokens;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Auth.RotateTokens;
using MixxFit.API.Infrastructure.Exceptions;
using MixxFit.API.Infrastructure.Persistence;
using Moq;

namespace MixxFit.UnitTests.Features.Auth;

public class RotateTokensTests : IDisposable
{
    private const string RawToken = "raw-refresh-token";
    private const string HashedToken = "hashed-refresh-token";
    private const string IpAddress = "127.0.0.1";

    private readonly AppDbContext _context;
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly Mock<ICurrentUserProvider> _currentUserProviderMock = new();
    private readonly RotateTokensHandler _handler;
    private readonly User _user = new() { Id = "user-1", UserName = "testuser", Email = "test@example.com" };

    public RotateTokensTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        var configMock = new Mock<IConfiguration>();
        var configSectionMock = new Mock<IConfigurationSection>();
        configSectionMock.Setup(x => x.Value).Returns("7");
        configMock.Setup(x => x.GetSection("RefreshConfig:ExpirationInDays")).Returns(configSectionMock.Object);

        _currentUserProviderMock.Setup(p => p.GetCurrentUserIpAddress()).Returns(IpAddress);
        _tokenServiceMock.Setup(s => s.HashToken(RawToken)).Returns(HashedToken);

        _handler = new RotateTokensHandler(
            _context,
            _tokenServiceMock.Object,
            _currentUserProviderMock.Object,
            configMock.Object
        );
    }

    [Fact]
    public async Task Handle_WhenTokenIsValid_ShouldRotateTokensAndReturnSuccess()
    {
        var oldToken = SeedRefreshToken();
        var newRawToken = "new-raw-refresh-token";
        var newHashedToken = "new-hashed-token";
        var expectedJwt = "generated-jwt-token";

        _tokenServiceMock
            .Setup(s => s.ValidateRefreshToken(It.Is<RefreshToken>(rt => rt.TokenHash == HashedToken)))
            .Returns(Result<string>.Success());
        _tokenServiceMock.Setup(s => s.CreateRefreshToken()).Returns(newRawToken);
        _tokenServiceMock.Setup(s => s.HashToken(newRawToken)).Returns(newHashedToken);
        _tokenServiceMock.Setup(s => s.GenerateJwtToken(It.IsAny<User>())).ReturnsAsync(expectedJwt);

        var result = await _handler.Handle(new RotateTokensRequest(RawToken));

        result.IsSuccess.Should().BeTrue();
        result.Payload.Should().NotBeNull();
        result.Payload!.AccessToken.Should().Be(expectedJwt);
        result.Payload.RefreshToken.Should().Be(newRawToken);

        oldToken.RevokedAt.Should().NotBeNull();
        oldToken.ReplacedByTokenHash.Should().Be(newHashedToken);

        var newToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.TokenHash == newHashedToken);
        newToken.Should().NotBeNull();
        newToken.UserId.Should().Be(_user.Id);
        newToken.CreatedByIp.Should().Be(IpAddress);
    }

    [Fact]
    public async Task Handle_WhenTokenIsExpired_ShouldReturnFailure()
    {
        SeedRefreshToken(expiresAt: DateTime.UtcNow.AddDays(-1));
        var expiredError = AuthError.ExpiredToken("Token has expired");

        _tokenServiceMock
            .Setup(s => s.ValidateRefreshToken(It.IsAny<RefreshToken>()))
            .Returns(Result<string>.Failure(expiredError));

        var result = await _handler.Handle(new RotateTokensRequest(RawToken));

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "Auth.ExpiredToken");
    }

    [Fact]
    public async Task Handle_WhenTokenDoesNotExist_ShouldReturnFailure()
    {
        var notFoundError = AuthError.ExpiredToken("Token does not exist");

        _tokenServiceMock
            .Setup(s => s.ValidateRefreshToken(null))
            .Returns(Result<string>.Failure(notFoundError));

        var result = await _handler.Handle(new RotateTokensRequest(RawToken));

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Code == "Auth.ExpiredToken");
    }

    [Fact]
    public async Task Handle_WhenSecurityBreachOccurs_ShouldRevokeAllTokensAndThrowException()
    {
        SeedRefreshToken(revokedAt: DateTime.UtcNow.AddDays(-1));
        var breachError = AuthError.SecurityBreach();

        _tokenServiceMock
            .Setup(s => s.ValidateRefreshToken(It.IsAny<RefreshToken>()))
            .Returns(Result<string>.Failure(breachError));

        var act = () => _handler.Handle(new RotateTokensRequest(RawToken));

        await act.Should().ThrowAsync<AllTokensRevokedException>();
        _tokenServiceMock.Verify(s => s.RevokeAllRefreshTokens(_user.Id), Times.Once);
    }

    private RefreshToken SeedRefreshToken(DateTime? expiresAt = null, DateTime? revokedAt = null)
    {
        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = _user.Id,
            User = _user,
            TokenHash = HashedToken,
            CreatedByIp = IpAddress,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt ?? DateTime.UtcNow.AddDays(7),
            RevokedAt = revokedAt
        };

        _context.Users.Add(_user);
        _context.RefreshTokens.Add(token);
        _context.SaveChanges();

        return token;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}