using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.RefreshTokens;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Infrastructure.Exceptions;
using MixxFit.API.Infrastructure.Persistence;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace MixxFit.API.Infrastructure.Security;

public class TokenService(
    UserManager<User> userManager, 
    IConfiguration configuration, 
    AppDbContext context,
    ICurrentUserProvider currentUserProvider) : ITokenService
{
    public async Task<TokenResponseDto> GenerateAuthTokens(User user)
    {
        var userIp = currentUserProvider.GetCurrentUserIpAddress();
        var refreshToken  = await AssignRefreshToken(user, userIp);

        var tokenResponse = new TokenResponseDto(await GenerateJwtToken(user), refreshToken);

        return tokenResponse;
    }

    public async Task<Result> RevokeRefreshToken(string oldToken)
    {
        var tokenHash = HashToken(oldToken);
        var token = await context.RefreshTokens
            .Where(rt => rt.TokenHash == tokenHash)
            .FirstOrDefaultAsync();

        if (token is null)
            return Result.Failure(AuthError.ExpiredToken("Token does not exist"));
        
        token.RevokedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public Result<string> ValidateRefreshToken(RefreshToken? oldToken)
    {
        if (oldToken is null)
            return Result<string>.Failure(AuthError.ExpiredToken("Token does not exist"));
        
        if (oldToken.ExpiresAt < DateTime.UtcNow)
            return Result<string>.Failure(AuthError.ExpiredToken());
        if(oldToken.RevokedAt is not null)
            return Result<string>.Failure(AuthError.SecurityBreach("Token has been revoked"));
        
        return Result<string>.Success(oldToken.TokenHash);
    }

    public async Task RevokeAllRefreshTokens(string userId)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .ExecuteUpdateAsync(rt => rt.SetProperty(x => x.RevokedAt, DateTime.UtcNow));
            await transaction.CommitAsync();
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync();
            throw new SecurityDbUpdateException(userId, "A database error occurred while revoking all refresh tokens.", ex);
        }
        
    }
    
    public async Task<string> GenerateJwtToken(User user)
    {
        var secretKey = configuration["JwtConfig:Token"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

        var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var rolesList = await userManager.GetRolesAsync(user);

        var newClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        
        newClaims.AddRange(rolesList.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(newClaims),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<double>("JwtConfig:ExpirationInMinutes")),
            SigningCredentials = signingCreds,
            Issuer = configuration["JwtConfig:Issuer"],
            Audience = configuration["JwtConfig:Audience"]
        };

        var token = new JsonWebTokenHandler().CreateToken(tokenDescriptor);

        return token;
    }

    public string CreateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
    
    private async Task<string> AssignRefreshToken(User user, string userIp)
    {

        var token = CreateRefreshToken();

        var newToken = new RefreshToken
        {
            UserId = user.Id,
            CreatedByIp = userIp,
            ExpiresAt = DateTime.UtcNow.AddDays(configuration.GetValue<int>("RefreshConfig:ExpirationInDays")),
            TokenHash = HashToken(token)
        };

        context.Add(newToken);
        await context.SaveChangesAsync();

        return token;
    }
    
    public string HashToken(string rawToken)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(rawToken);
        byte[] hashBytes = SHA256.HashData(inputBytes);
        
        return Convert.ToBase64String(hashBytes); 
    }
    
    
    
}