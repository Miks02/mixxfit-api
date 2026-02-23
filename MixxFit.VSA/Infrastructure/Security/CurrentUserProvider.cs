using System.Security.Claims;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Infrastructure.Security;

public class CurrentUserProvider(IHttpContextAccessor contextAccessor) : ICurrentUserProvider
{
    public string GetCurrentUserId()
        => contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) 
           ?? throw new UnauthorizedAccessException();
}