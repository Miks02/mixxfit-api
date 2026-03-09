using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Infrastructure.Security;

public class CookieProvider(IHttpContextAccessor contextAccessor) : ICookieProvider
{
    public string GetRefreshTokenCookie() => contextAccessor.HttpContext?.Request.Cookies["refreshToken"] 
                                             ?? "";
    public void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = GetCookieOptions();
        cookieOptions.Expires = DateTime.UtcNow.AddDays(7);
        
        contextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
    
    public void DeleteRefreshTokenCookie()
    {
        var cookieOptions = GetCookieOptions();
        contextAccessor.HttpContext!.Response.Cookies.Delete("refreshToken", cookieOptions);
    }

    private CookieOptions GetCookieOptions()
    {
        return new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/"
        };
    }
}