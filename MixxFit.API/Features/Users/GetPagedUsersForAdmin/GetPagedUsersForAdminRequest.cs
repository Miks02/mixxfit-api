using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace MixxFit.API.Features.Users.GetPagedUsersForAdmin;

public record GetPagedUsersForAdminRequest
{
    [FromQuery(Name = "page")]
    [DefaultValue(1)]
    public int Page { get; init; } = 1;

    [FromQuery(Name = "pageSize")]
    [DefaultValue(20)]
    public int PageSize { get; init; } = 20;

    [FromQuery(Name = "search")]
    public string? Search { get; init; }

    [FromQuery(Name = "sort")]
    [DefaultValue("newest")]
    public string? Sort { get; init; }

    [FromQuery(Name = "isDeleted")]
    public bool? IsDeleted { get; init; }
}
