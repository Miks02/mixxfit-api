using System.Net;
using System.Security.Claims;
using AwesomeAssertions;
using Microsoft.AspNetCore.Http;
using MixxFit.API.Infrastructure.Security;
using Moq;

namespace MixxFit.UnitTests.Infrastructure.Security;

public class CurrentUserProviderTests
{
    private const string UserId = "user-1";

    private readonly Mock<IHttpContextAccessor> _contextAccessorMock = new();
    private readonly DefaultHttpContext _httpContext = new();
    private readonly CurrentUserProvider _provider;

    public CurrentUserProviderTests()
    {
        _contextAccessorMock.Setup(a => a.HttpContext).Returns(_httpContext);
        _provider = new CurrentUserProvider(_contextAccessorMock.Object);
    }

    [Fact]
    public void GetCurrentUserId_WhenNameIdentifierClaimExists_ShouldReturnUserId()
    {
        SetUserClaims(new Claim(ClaimTypes.NameIdentifier, UserId));

        var userId = _provider.GetCurrentUserId();

        userId.Should().Be(UserId);
    }

    [Fact]
    public void GetCurrentUserId_WhenUserHasOtherClaimsOnly_ShouldThrowUnauthorizedAccessException()
    {
        SetUserClaims(new Claim(ClaimTypes.Email, "user1@mail.com"), new Claim(ClaimTypes.Role, "User"));

        var act = () => _provider.GetCurrentUserId();

        act.Should().Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void GetCurrentUserId_WhenUserIsNotAuthenticated_ShouldThrowUnauthorizedAccessException()
    {
        var act = () => _provider.GetCurrentUserId();

        act.Should().Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void GetCurrentUserId_WhenHttpContextIsNull_ShouldThrowUnauthorizedAccessException()
    {
        _contextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext?)null);

        var act = () => _provider.GetCurrentUserId();

        act.Should().Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void GetCurrentUserIpAddress_WhenRemoteIpIsIPv4_ShouldReturnIpAddress()
    {
        _httpContext.Connection.RemoteIpAddress = IPAddress.Parse("192.168.1.10");

        var ipAddress = _provider.GetCurrentUserIpAddress();

        ipAddress.Should().Be("192.168.1.10");
    }

    [Fact]
    public void GetCurrentUserIpAddress_WhenRemoteIpIsIPv6_ShouldReturnIpAddress()
    {
        _httpContext.Connection.RemoteIpAddress = IPAddress.IPv6Loopback;

        var ipAddress = _provider.GetCurrentUserIpAddress();

        ipAddress.Should().Be("::1");
    }

    [Fact]
    public void GetCurrentUserIpAddress_WhenRemoteIpIsNull_ShouldReturnUnknownIp()
    {
        _httpContext.Connection.RemoteIpAddress = null;

        var ipAddress = _provider.GetCurrentUserIpAddress();

        ipAddress.Should().Be("Unknown IP");
    }

    [Fact]
    public void GetCurrentUserIpAddress_WhenHttpContextIsNull_ShouldReturnUnknownIp()
    {
        _contextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext?)null);

        var ipAddress = _provider.GetCurrentUserIpAddress();

        ipAddress.Should().Be("Unknown IP");
    }

    private void SetUserClaims(params Claim[] claims)
    {
        _httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
    }
}
