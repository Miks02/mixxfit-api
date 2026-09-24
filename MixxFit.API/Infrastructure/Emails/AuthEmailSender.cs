using System.Text.Encodings.Web;
using MixxFit.API.Common.Interfaces;
using Resend;

namespace MixxFit.API.Infrastructure.Emails;

public class AuthEmailSender(IConfiguration configuration, IResend resend) : IAuthEmailSender
{
    private readonly string _clientUrl = configuration["Web:ClientUrl"]!;
    private readonly string _fromEmail = configuration["Resend:EmailSender"]!;

    public async Task SendPasswordResetEmailAsync(string email, string userId, string token)
    {
        token = UrlEncoder.Default.Encode(token);

        var resetUrl = $"{_clientUrl}/reset-password?token={token}&userId={userId}";

        var htmlBody = $"""
                         <!DOCTYPE html>
                         <html lang="en">
                         <head>
                             <meta charset="UTF-8">
                             <meta name="viewport" content="width=device-width, initial-scale=1.0">
                             <meta name="color-scheme" content="light only">
                             <meta name="supported-color-schemes" content="light only">
                             <title>Password reset</title>
                         </head>
                         <body style="margin: 0; padding: 24px 12px; background-color: #64748b; font-family: 'Inter', Arial, Helvetica, sans-serif; color: #1f2937;">
                             <div style="max-width: 560px; margin: 0 auto; border-radius: 16px; overflow: hidden; box-shadow: 0 10px 25px rgba(0, 0, 0, 0.4);">
                         
                                 <div style="background-color: #eab308; background-image: linear-gradient(to bottom right, #eab308, #ca8a04); padding: 28px 32px; text-align: center;">
                                     <span style="font-size: 32px; font-weight: 700; letter-spacing: 0.5px; color: #111827;">Mixx<span style="color: #1e293b;">Fit</span></span>
                                 </div>
                         
                                 <div style="background-color: #d1d5db; padding: 40px 32px; text-align: center;">
                                     <h1 style="margin: 0 0 16px; font-size: 32px; font-weight: 600; color: #1f2937;">Reset your password</h1>
                                     <p style="margin: 0 0 8px; font-size: 14px; line-height: 1.6; color: #4b5563;">
                                         We received a request to reset the password for your account.
                                     </p>
                                     <p style="margin: 0 0 32px; font-size: 14px; line-height: 1.6; color: #4b5563;">
                                         Click the button below to choose a new one and get back to your training.
                                     </p>
                         
                                     <a href="{resetUrl}"
                                        style="display: inline-block; padding: 14px 32px; border-radius: 6px; background-color: #fbbf24; background-image: linear-gradient(to bottom right, #fbbf24, #f59e0b); color: #1e293b; font-size: 18px; font-weight: 600; text-decoration: none; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);">
                                         Reset password
                                     </a>
                         
                                     <p style="margin: 24px 0 0; font-size: 13px; color: #4b5563;">
                                         This link is valid for <strong style="color: #1f2937;">1 hour</strong>.
                                     </p>
                                 </div>
                         
                                 <div style="background-color: #e5e7eb; padding: 24px 32px;">
                                     <p style="margin: 0 0 16px; font-size: 13px; line-height: 1.6; color: #4b5563;">
                                         If you didn't request a password change, you can safely ignore this email. Your password will remain unchanged.
                                     </p>
                                     <p style="margin: 0; font-size: 12px; line-height: 1.6; color: #6b7280; word-break: break-all;">
                                         Button not working? Try the link below. Right click on the link and copy it via 'copy link' option:<br/>
                                         <a href="{resetUrl}" style="color: #ca8a04; text-decoration: underline;">Password reset link</a>
                                     </p>
                                 </div>
                         
                             </div>
                         </body>
                         </html>
                         """;

        var message = new EmailMessage
        {
            From = _fromEmail,
            To = email,
            HtmlBody = htmlBody,
            Subject = "Reset your MixxFit password"
        };

        await resend.EmailSendAsync(message);
    }
}
