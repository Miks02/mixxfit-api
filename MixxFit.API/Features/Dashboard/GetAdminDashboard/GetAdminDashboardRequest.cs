using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Features.Dashboard.GetAdminDashboard;

public record GetAdminDashboardRequest
{
    [FromQuery(Name = "page")]
    [DefaultValue(1)]
    public int Page { get; init; } = 1;

    [FromQuery(Name = "pageSize")]
    [DefaultValue(20)]
    public int PageSize { get; init; } = 20;
}
