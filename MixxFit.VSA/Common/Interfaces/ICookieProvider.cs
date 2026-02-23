namespace MixxFit.VSA.Common.Interfaces;

public interface ICookieProvider
{
    public string GetRefreshTokenCookie();
    public void SetRefreshTokenCookie(string refreshToken);
    public void DeleteRefreshTokenCookie();
}