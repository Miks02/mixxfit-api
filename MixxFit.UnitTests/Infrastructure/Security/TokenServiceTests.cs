using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.RefreshTokens;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Infrastructure.Security;
using MixxFit.UnitTests.TestUtilities;
using Moq;

namespace MixxFit.UnitTests.Infrastructure.Security;

public class TokenServiceTests : IDisposable
{
    private const string UserId = "user-1";
    private const string OtherUserId = "user-2";
    private const string IpAddress = "127.0.0.1";
    private const string JwtSecret = "unit-test-signing-key-that-is-long-enough-for-hmac-sha256";
    private const string Issuer = "mixxfit-tests";
    private const string Audience = "mixxfit-tests-client";
    private const double JwtExpirationInMinutes = 15;
    private const int RefreshExpirationInDays = 7;

    private readonly SqliteTestDatabase _database = new();
    private readonly AppDbContext _context;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ICurrentUserProvider> _currentUserProviderMock = new();
    private readonly TokenService _tokenService;
    private readonly User _user = new() { Id = UserId, UserName = "user1", Email = "user1@mail.com" };

    public TokenServiceTests()
    {
        using (var seedContext = _database.CreateContext())
        {
            seedContext.Users.AddRange(
                new User { Id = UserId, UserName = "user1", Email = "user1@mail.com", FitnessProfile = new FitnessProfile { UserId = UserId } },
                new User { Id = OtherUserId, UserName = "user2", Email = "user2@mail.com", FitnessProfile = new FitnessProfile { UserId = OtherUserId } });
            seedContext.SaveChanges();
        }

        _context = _database.CreateContext();

        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<User>())).ReturnsAsync(new List<string> { "User" });

        _currentUserProviderMock.Setup(p => p.GetCurrentUserIpAddress()).Returns(IpAddress);

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtConfig:Token"] = JwtSecret,
                ["JwtConfig:Issuer"] = Issuer,
                ["JwtConfig:Audience"] = Audience,
                ["JwtConfig:ExpirationInMinutes"] = "15",
                ["RefreshConfig:ExpirationInDays"] = "7"
            })
            .Build();

        _tokenService = new TokenService(
            _userManagerMock.Object,
            configuration,
            _context,
            _currentUserProviderMock.Object);
    }

    public void Dispose()
    {
        _context.Dispose();
        _database.Dispose();
    }

    [Fact]
    public void HashToken_WhenCalled_ShouldReturnBase64EncodedSha256Hash()
    {
        var expected = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("raw-refresh-token")));

        var hash = _tokenService.HashToken("raw-refresh-token");

        hash.Should().Be(expected);
        hash.Should().NotBe("raw-refresh-token");
    }

    [Fact]
    public void HashToken_WhenCalledTwiceWithSameInput_ShouldReturnSameHash()
    {
        var first = _tokenService.HashToken("raw-refresh-token");
        var second = _tokenService.HashToken("raw-refresh-token");

        first.Should().Be(second);
    }

    [Fact]
    public void HashToken_WhenInputsDiffer_ShouldReturnDifferentHashes()
    {
        var first = _tokenService.HashToken("raw-refresh-token");
        var second = _tokenService.HashToken("raw-refresh-token2");

        first.Should().NotBe(second);
    }

    [Fact]
    public void CreateRefreshToken_WhenCalled_ShouldReturnBase64StringOf32Bytes()
    {
        var token = _tokenService.CreateRefreshToken();

        Convert.FromBase64String(token).Should().HaveCount(32);
    }

    [Fact]
    public void CreateRefreshToken_WhenCalledMultipleTimes_ShouldReturnUniqueTokens()
    {
        var tokens = Enumerable.Range(0, 20).Select(_ => _tokenService.CreateRefreshToken()).ToList();

        tokens.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void ValidateRefreshToken_WhenTokenIsNull_ShouldReturnFailure()
    {
        var result = _tokenService.ValidateRefreshToken(null);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(AuthError.ExpiredToken("Token does not exist"));
    }

    [Fact]
    public void ValidateRefreshToken_WhenTokenIsExpired_ShouldReturnFailure()
    {
        var token = CreateRefreshToken("hash", expiresAt: DateTime.UtcNow.AddMinutes(-1));

        var result = _tokenService.ValidateRefreshToken(token);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(AuthError.ExpiredToken());
    }

    [Fact]
    public void ValidateRefreshToken_WhenTokenIsRevoked_ShouldReturnSecurityBreachFailure()
    {
        var token = CreateRefreshToken("hash", revokedAt: DateTime.UtcNow.AddMinutes(-1));

        var result = _tokenService.ValidateRefreshToken(token);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(AuthError.SecurityBreach());
    }

    [Fact]
    public void ValidateRefreshToken_WhenTokenIsExpiredAndRevoked_ShouldReturnExpiredFailure()
    {
        var token = CreateRefreshToken(
            "hash",
            expiresAt: DateTime.UtcNow.AddDays(-1),
            revokedAt: DateTime.UtcNow.AddDays(-2));

        var result = _tokenService.ValidateRefreshToken(token);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(AuthError.ExpiredToken());
    }

    [Fact]
    public void ValidateRefreshToken_WhenTokenIsValid_ShouldReturnSuccessWithTokenHash()
    {
        var token = CreateRefreshToken("valid-hash");

        var result = _tokenService.ValidateRefreshToken(token);

        result.IsSuccess.Should().BeTrue();
        result.Payload.Should().Be("valid-hash");
    }

    [Fact]
    public async Task RevokeRefreshToken_WhenTokenDoesNotExist_ShouldReturnFailure()
    {
        var result = await _tokenService.RevokeRefreshToken("unknown-token");

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(AuthError.ExpiredToken("Token does not exist"));
    }

    [Fact]
    public async Task RevokeRefreshToken_WhenTokenExists_ShouldSetRevokedAtAndReturnSuccess()
    {
        SeedRefreshToken(_tokenService.HashToken("raw-refresh-token"));

        var result = await _tokenService.RevokeRefreshToken("raw-refresh-token");

        result.IsSuccess.Should().BeTrue();

        await using var assertContext = _database.CreateContext();
        var token = await assertContext.RefreshTokens.SingleAsync();
        token.RevokedAt.Should().NotBeNull();
        token.RevokedAt!.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task RevokeRefreshToken_WhenTokenExists_ShouldNotRevokeOtherTokens()
    {
        var otherHash = _tokenService.HashToken("other-refresh-token");
        SeedRefreshToken(_tokenService.HashToken("raw-refresh-token"));
        SeedRefreshToken(otherHash);

        await _tokenService.RevokeRefreshToken("raw-refresh-token");

        await using var assertContext = _database.CreateContext();
        var otherToken = await assertContext.RefreshTokens.SingleAsync(rt => rt.TokenHash == otherHash);
        otherToken.RevokedAt.Should().BeNull();
    }

    [Fact]
    public async Task RevokeAllRefreshTokens_WhenUserHasTokens_ShouldRevokeAllOfThem()
    {
        SeedRefreshToken("hash-1");
        SeedRefreshToken("hash-2");

        await _tokenService.RevokeAllRefreshTokens(UserId);

        await using var assertContext = _database.CreateContext();
        var tokens = await assertContext.RefreshTokens.Where(rt => rt.UserId == UserId).ToListAsync();
        tokens.Should().HaveCount(2);
        tokens.Should().OnlyContain(rt => rt.RevokedAt != null);
    }

    [Fact]
    public async Task RevokeAllRefreshTokens_WhenOtherUserHasTokens_ShouldNotRevokeThem()
    {
        SeedRefreshToken("hash-1");
        SeedRefreshToken("other-hash", userId: OtherUserId);

        await _tokenService.RevokeAllRefreshTokens(UserId);

        await using var assertContext = _database.CreateContext();
        var otherToken = await assertContext.RefreshTokens.SingleAsync(rt => rt.UserId == OtherUserId);
        otherToken.RevokedAt.Should().BeNull();
    }

    [Fact]
    public async Task RevokeAllRefreshTokens_WhenUserHasNoTokens_ShouldNotThrow()
    {
        var act = () => _tokenService.RevokeAllRefreshTokens(UserId);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task GenerateJwtToken_WhenCalled_ShouldContainUserClaims()
    {
        var jwt = ReadToken(await _tokenService.GenerateJwtToken(_user));

        jwt.Subject.Should().Be(UserId);
        jwt.GetClaim(JwtRegisteredClaimNames.Email).Value.Should().Be("user1@mail.com");
        jwt.GetClaim(ClaimTypes.NameIdentifier).Value.Should().Be(UserId);
        Guid.TryParse(jwt.Id, out _).Should().BeTrue();
    }

    [Fact]
    public async Task GenerateJwtToken_WhenUserHasRoles_ShouldContainRoleClaimForEachRole()
    {
        _userManagerMock
            .Setup(m => m.GetRolesAsync(_user))
            .ReturnsAsync(new List<string> { "User", "Admin" });

        var jwt = ReadToken(await _tokenService.GenerateJwtToken(_user));

        jwt.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value)
            .Should().BeEquivalentTo("User", "Admin");
    }

    [Fact]
    public async Task GenerateJwtToken_WhenUserHasNoRoles_ShouldNotContainRoleClaims()
    {
        _userManagerMock
            .Setup(m => m.GetRolesAsync(_user))
            .ReturnsAsync(new List<string>());

        var jwt = ReadToken(await _tokenService.GenerateJwtToken(_user));

        jwt.Claims.Should().NotContain(c => c.Type == ClaimTypes.Role);
    }

    [Fact]
    public async Task GenerateJwtToken_WhenCalled_ShouldUseConfiguredIssuerAudienceAndExpiration()
    {
        var jwt = ReadToken(await _tokenService.GenerateJwtToken(_user));

        jwt.Issuer.Should().Be(Issuer);
        jwt.Audiences.Should().ContainSingle().Which.Should().Be(Audience);
        jwt.ValidTo.Should().BeCloseTo(DateTime.UtcNow.AddMinutes(JwtExpirationInMinutes), TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task GenerateJwtToken_WhenCalled_ShouldBeSignedWithConfiguredSecret()
    {
        var token = await _tokenService.GenerateJwtToken(_user);

        var validResult = await ValidateSignature(token, JwtSecret);
        var invalidResult = await ValidateSignature(token, "a-completely-different-signing-key-of-sufficient-length");

        validResult.IsValid.Should().BeTrue();
        invalidResult.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task GenerateJwtToken_WhenCalledTwice_ShouldGenerateDifferentJti()
    {
        var first = ReadToken(await _tokenService.GenerateJwtToken(_user));
        var second = ReadToken(await _tokenService.GenerateJwtToken(_user));

        first.Id.Should().NotBe(second.Id);
    }

    [Fact]
    public async Task GenerateAuthTokens_WhenCalled_ShouldReturnAccessAndRefreshToken()
    {
        var tokens = await _tokenService.GenerateAuthTokens(_user);

        ReadToken(tokens.AccessToken).Subject.Should().Be(UserId);
        Convert.FromBase64String(tokens.RefreshToken).Should().HaveCount(32);
    }

    [Fact]
    public async Task GenerateAuthTokens_WhenCalled_ShouldPersistHashedRefreshToken()
    {
        var tokens = await _tokenService.GenerateAuthTokens(_user);

        await using var assertContext = _database.CreateContext();
        var stored = await assertContext.RefreshTokens.SingleAsync();
        stored.TokenHash.Should().Be(_tokenService.HashToken(tokens.RefreshToken));
        stored.TokenHash.Should().NotBe(tokens.RefreshToken);
        stored.UserId.Should().Be(UserId);
        stored.CreatedByIp.Should().Be(IpAddress);
        stored.RevokedAt.Should().BeNull();
        stored.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddDays(RefreshExpirationInDays), TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task GenerateAuthTokens_WhenCalledTwice_ShouldPersistSeparateRefreshTokens()
    {
        var first = await _tokenService.GenerateAuthTokens(_user);
        var second = await _tokenService.GenerateAuthTokens(_user);

        first.RefreshToken.Should().NotBe(second.RefreshToken);

        await using var assertContext = _database.CreateContext();
        (await assertContext.RefreshTokens.CountAsync(rt => rt.UserId == UserId)).Should().Be(2);
    }

    private static JsonWebToken ReadToken(string token) => new JsonWebTokenHandler().ReadJsonWebToken(token);

    private static Task<TokenValidationResult> ValidateSignature(string token, string secret) =>
        new JsonWebTokenHandler().ValidateTokenAsync(token, new TokenValidationParameters
        {
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        });

    private static RefreshToken CreateRefreshToken(
        string tokenHash,
        string userId = UserId,
        DateTime? expiresAt = null,
        DateTime? revokedAt = null) => new()
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        TokenHash = tokenHash,
        CreatedByIp = IpAddress,
        CreatedAt = DateTime.UtcNow,
        ExpiresAt = expiresAt ?? DateTime.UtcNow.AddDays(RefreshExpirationInDays),
        RevokedAt = revokedAt
    };

    private void SeedRefreshToken(string tokenHash, string userId = UserId)
    {
        using var seedContext = _database.CreateContext();
        seedContext.RefreshTokens.Add(CreateRefreshToken(tokenHash, userId));
        seedContext.SaveChanges();
    }
}
