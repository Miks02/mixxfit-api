using FluentValidation;

namespace MixxFit.API.Features.Dashboard.GetAdminDashboard;

public class GetAdminDashboardValidator : AbstractValidator<GetAdminDashboardRequest>
{
    public GetAdminDashboardValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");
    }
}
