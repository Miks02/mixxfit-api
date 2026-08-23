using System.Security.Claims;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Infrastructure.Security;

public class CurrentUserProvider(IHttpContextAccessor contextAccessor) : ICurrentUserProvider
{
    public string GetCurrentUserId()
        => contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) 
           ?? throw new UnauthorizedAccessException();
    
    public string GetCurrentUserIpAddress()
        => contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() 
           ?? "Unknown IP";
}