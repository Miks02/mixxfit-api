namespace MixxFit.VSA.Common.Results;

public record PagedResult<T>(int Page, int PageSize, IReadOnlyList<T> Items, int TotalCount, int PaginatedCount)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
};