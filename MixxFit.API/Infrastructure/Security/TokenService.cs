using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Domain.Entities.Users;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace MixxFit.API.Infrastructure.Security;

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

    private static string CreateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
    
    private async Task<Result<string>> AssignRefreshToken(User user)
    {
        user.RefreshToken = CreateRefreshToken();
        user.TokenExpDate = DateTime.UtcNow.AddDays(7);

        return (await userManager.UpdateAsync(user)).HandleIdentityResult(user.RefreshToken);
    }
    
    
    
    
    
}