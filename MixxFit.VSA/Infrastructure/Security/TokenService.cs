using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MixxFit.VSA.Common.Extensions;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Common.Results;
using MixxFit.VSA.Domain.Entities;

namespace MixxFit.VSA.Infrastructure.Security;

public class TokenService(UserManager<User> userManager, IConfiguration configuration) : ITokenService
{
    public async Task<Result<TokenResponseDto>> GenerateAuthTokens(User user)
    {
        var assignRefreshToken = await AssignRefreshToken(user);

        if (!assignRefreshToken.IsSuccess)
            return Result<TokenResponseDto>.Failure(assignRefreshToken.Errors.ToArray());

        var tokenResponse = new TokenResponseDto(await GenerateJwtToken(user), assignRefreshToken.Payload!);

        return Result<TokenResponseDto>.Success(tokenResponse);
    }
    
    private async Task<string> GenerateJwtToken(User user)
    {
        var rolesList = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, user.Id),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (ClaimTypes.NameIdentifier, user.Id),
        };

        claims.AddRange(rolesList.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Token"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            issuer: configuration["JwtConfig:Issuer"],
            audience: configuration["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<double>("JwtConfig:ExpirationInMinutes")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string CreateRefreshToken()
    {
        var randomBytes = new Byte[32];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }
    
    private async Task<Result<string>> AssignRefreshToken(User user)
    {
        user.RefreshToken = CreateRefreshToken();
        user.TokenExpDate = DateTime.UtcNow.AddDays(7);

        return (await userManager.UpdateAsync(user)).HandleIdentityResult(user.RefreshToken);
    }
    
    
    
    
    
}