namespace MixxFit.API.Common.Interfaces;

public interface ICookieProvider
{
    public string GetRefreshTokenCookie();
    public string GetUserIp();
    public void SetRefreshTokenCookie(string refreshToken);
    public void DeleteRefreshTokenCookie();
}