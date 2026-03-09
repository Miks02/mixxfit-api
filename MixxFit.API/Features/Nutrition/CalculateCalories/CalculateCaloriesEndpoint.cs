using System.Net;
using System.Reflection.Metadata.Ecma335;
using MixxFit.API.Common.Interfaces;

namespace MixxFit.API.Features.Nutrition.CalculateCalories;

public class CalculateCaloriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/nutrition/calorie-goals", (CalculateCaloriesRequest request, CalculateCaloriesHandler handler) => handler.Handle(request))
        .WithTags("Nutrition")
        .RequireAuthorization()
        .ProducesValidationProblem();
        
        
    }
}