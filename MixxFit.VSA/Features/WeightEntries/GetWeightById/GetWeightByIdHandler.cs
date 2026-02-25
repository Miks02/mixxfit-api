using Microsoft.EntityFrameworkCore;
using MixxFit.VSA.Common.Interfaces;
using MixxFit.VSA.Infrastructure.Persistence;

namespace MixxFit.VSA.Features.WeightEntries.GetWeightById;

public class GetWeightByIdHandler(AppDbContext context) : IHandler
{
    public async Task<GetWeightByIdResponse?> Handle(string userId, int id, CancellationToken cancellationToken)
    {
        return await context.WeightEntries
            .AsNoTracking()
            .Where(w => w.Id == id && w.UserId == userId)
            .Select(w => new GetWeightByIdResponse()
            {
                Id = w.Id,
                Weight = w.Weight,
                Notes = w.Notes,
                Time = w.Time,
                CreatedAt = w.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}