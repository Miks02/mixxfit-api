using AwesomeAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using MixxFit.API.Infrastructure.Security;
using Moq;

namespace MixxFit.UnitTests.Infrastructure.Security;

public class CookieProviderTests
{
    private const string CookieName = "refreshToken";
    private const string RefreshToken = "raw-refresh-token";
    private const int RefreshExpirationInDays = 7;

    private readonly Mock<IHttpContextAccessor> _contextAccessorMock = new();
    private readonly DefaultHttpContext _httpContext = new();
    private readonly CookieProvider _provider;

    public CookieProviderTests()
    {
        _contextAccessorMock.Setup(a => a.HttpContext).Returns(_httpContext);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["RefreshConfig:ExpirationInDays"] = "7"
            })
            .Build();

        _provider = new CookieProvider(_contextAccessorMock.Object, configuration);
    }

    [Fact]
    public void GetRefreshTokenCookie_WhenCookieExists_ShouldReturnCookieValue()
    {
        _httpContext.Request.Headers.Cookie = $"{CookieName}={RefreshToken}";

        var cookie = _provider.GetRefreshTokenCookie();

        cookie.Should().Be(RefreshToken);
    }

    [Fact]
    public void GetRefreshTokenCookie_WhenOtherCookiesExist_ShouldReturnOnlyRefreshToken()
    {
        _httpContext.Request.Headers.Cookie = $"theme=dark; {CookieName}={RefreshToken}; lang=sr";

        var cookie = _provider.GetRefreshTokenCookie();

        cookie.Should().Be(RefreshToken);
    }

    [Fact]
    public void GetRefreshTokenCookie_WhenCookieDoesNotExist_ShouldReturnEmptyString()
    {
        _httpContext.Request.Headers.Cookie = "theme=dark";

        var cookie = _provider.GetRefreshTokenCookie();

        cookie.Should().BeEmpty();
    }

    [Fact]
    public void GetRefreshTokenCookie_WhenHttpContextIsNull_ShouldReturnEmptyString()
    {
        _contextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext?)null);

        var cookie = _provider.GetRefreshTokenCookie();

        cookie.Should().BeEmpty();
    }

    [Fact]
    public void SetRefreshTokenCookie_WhenCalled_ShouldAppendRefreshTokenCookie()
    {
        _provider.SetRefreshTokenCookie(RefreshToken);

        var cookie = ReadResponseCookie();
        cookie.Name.Value.Should().Be(CookieName);
        cookie.Value.Value.Should().Be(RefreshToken);
    }

    [Fact]
    public void SetRefreshTokenCookie_WhenTokenContainsBase64Characters_ShouldRoundTripValue()
    {
        const string base64Token = "abc+def/ghi=";

        _provider.SetRefreshTokenCookie(base64Token);

        var cookie = ReadResponseCookie();
        Uri.UnescapeDataString(cookie.Value.Value!).Should().Be(base64Token);
    }

    [Fact]
    public void SetRefreshTokenCookie_WhenCalled_ShouldUseSecureCookieOptions()
    {
        _provider.SetRefreshTokenCookie(RefreshToken);

        AssertSecureCookieOptions(ReadResponseCookie());
    }

    [Fact]
    public void SetRefreshTokenCookie_WhenCalled_ShouldExpireAfterConfiguredNumberOfDays()
    {
        _provider.SetRefreshTokenCookie(RefreshToken);

        var cookie = ReadResponseCookie();
        cookie.Expires.Should().NotBeNull();
        cookie.Expires!.Value.UtcDateTime.Should()
            .BeCloseTo(DateTime.UtcNow.AddDays(RefreshExpirationInDays), TimeSpan.FromSeconds(10));
    }

    [Fact]
    public void DeleteRefreshTokenCookie_WhenCalled_ShouldExpireRefreshTokenCookie()
    {
        _provider.DeleteRefreshTokenCookie();

        var cookie = ReadResponseCookie();
        cookie.Name.Value.Should().Be(CookieName);
        cookie.Value.Value.Should().BeEmpty();
        cookie.Expires.Should().NotBeNull();
        cookie.Expires!.Value.UtcDateTime.Should().BeBefore(DateTime.UtcNow);
    }

    [Fact]
    public void DeleteRefreshTokenCookie_WhenCalled_ShouldUseSameCookieOptionsAsSet()
    {
        _provider.DeleteRefreshTokenCookie();

        AssertSecureCookieOptions(ReadResponseCookie());
    }

    private SetCookieHeaderValue ReadResponseCookie()
    {
        var headers = _httpContext.Response.Headers.SetCookie;
        headers.Should().ContainSingle();

        return SetCookieHeaderValue.Parse(headers.ToString());
    }

    private static void AssertSecureCookieOptions(SetCookieHeaderValue cookie)
    {
        cookie.HttpOnly.Should().BeTrue();
        cookie.Secure.Should().BeTrue();
        cookie.SameSite.Should().Be(Microsoft.Net.Http.Headers.SameSiteMode.None);
        cookie.Path.Value.Should().Be("/");
    }
}
