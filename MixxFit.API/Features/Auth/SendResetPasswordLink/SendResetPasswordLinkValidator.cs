using FluentValidation;

namespace MixxFit.API.Features.Auth.SendResetPasswordLink;

public class SendResetPasswordLinkValidator : AbstractValidator<SendResetPasswordLinkRequest>
{
    public SendResetPasswordLinkValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}
