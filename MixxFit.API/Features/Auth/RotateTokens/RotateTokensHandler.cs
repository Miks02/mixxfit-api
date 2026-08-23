using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities.RefreshTokens;
using MixxFit.API.Infrastructure.Exceptions;
using MixxFit.API.Infrastructure.Persistence;

namespace MixxFit.API.Features.Auth.RotateTokens;

public class RotateTokensHandler(
    AppDbContext context,
    ITokenService tokenService,
    ICurrentUserProvider userProvider, 
    IConfiguration configuration) : IHandler
{
    public async Task<Result<RotateTokensResponse>> Handle(RotateTokensRequest request)
    {
        var oldToken = await context.RefreshTokens
            .Include(rt => rt.User)
            .Where(rt => rt.TokenHash == tokenService.HashToken(request.RefreshToken))
            .FirstOrDefaultAsync();

        var validationResult = tokenService.ValidateRefreshToken(oldToken);

        if (!validationResult.IsSuccess)
        {
            var error = validationResult.Errors.FirstOrDefault()!;
            if (error.Code == "Auth.ExpiredToken")
                return Result<RotateTokensResponse>.Failure(error);
            if (error.Code == "Auth.SecurityBreach")
            {
                await tokenService.RevokeAllRefreshTokens(oldToken!.UserId);
                throw new AllTokensRevokedException();
            }
            return Result<RotateTokensResponse>.Failure(error);
        }

        var unhashedToken = tokenService.CreateRefreshToken();

        var newToken = new RefreshToken
        {
            TokenHash = tokenService.HashToken(unhashedToken),
            UserId = oldToken!.UserId,
            CreatedByIp = userProvider.GetCurrentUserIpAddress(),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(configuration.GetValue<int>("RefreshConfig:ExpirationInDays"))
        };
        
        oldToken.RevokedAt = DateTime.UtcNow;
        oldToken.ReplacedByTokenHash = newToken.TokenHash;

        context.Add(newToken);
        await context.SaveChangesAsync();

        var jwt = await tokenService.GenerateJwtToken(oldToken.User);

        var response = new RotateTokensResponse(jwt, unhashedToken);

        return Result<RotateTokensResponse>.Success(response);
    }
}