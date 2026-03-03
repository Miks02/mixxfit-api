using System.Net;
using System.Reflection.Metadata.Ecma335;
using MixxFit.VSA.Common.Interfaces;

namespace MixxFit.VSA.Features.Calories.CalculateCalories;

public class CalculateCaloriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/calories", (CalculateCaloriesRequest request, CalculateCaloriesHandler handler) => handler.Handle(request))
        .WithTags("Calories")
        .RequireAuthorization()
        .ProducesValidationProblem();
        
        
    }
}