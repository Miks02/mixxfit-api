namespace MixxFit.API.Common.Interfaces;

public interface IAuthEmailSender
{
    Task SendPasswordResetEmailAsync(string email, string userId, string token);
    Task SendPasswordChangedEmailAsync(string email);
    Task SendAccountDeactivatedEmailAsync(string email);
    Task SendAccountReactivatedEmail(string email);
    Task SendAccountDeletedEmailAsync(string email);
}
