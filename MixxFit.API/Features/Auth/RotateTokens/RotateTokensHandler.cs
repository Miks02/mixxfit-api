using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Common.Results;
using MixxFit.API.Domain.Entities;
using MixxFit.API.Domain.ErrorCatalog;

namespace MixxFit.API.Features.Auth.RotateTokens;

public class RotateTokensHandler(UserManager<User> userManager, ITokenService tokenService) : IHandler
{
    public async Task<Result<RotateTokensResponse>> Handle(RotateTokensRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users
            .Where(u => u.RefreshToken == request.RefreshToken)
            .FirstOrDefaultAsync(cancellationToken);
        
        if(user is null || user.RefreshToken is null || user.TokenExpDate < DateTime.UtcNow)
            return Result<RotateTokensResponse>.Failure(AuthError.ExpiredToken());

        var tokensResult = await tokenService.GenerateAuthTokens(user);
        
        if(!tokensResult.IsSuccess)
            return Result<RotateTokensResponse>.Failure(tokensResult.Errors.ToArray());

        var response = new RotateTokensResponse(tokensResult.Payload!.AccessToken, tokensResult.Payload.RefreshToken);

        return Result<RotateTokensResponse>.Success(response);
    }
}