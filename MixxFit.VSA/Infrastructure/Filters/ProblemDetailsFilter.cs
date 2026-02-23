using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MixxFit.VSA.Common.Results;

namespace MixxFit.VSA.Infrastructure.Filters;

public class ProblemDetailsFilter(ILogger<ProblemDetailsFilter> logger) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        
        var innerResult = result is INestedHttpResult nested
            ? nested.Result
            : result as IResult;
        
        if (innerResult is not BadRequest<Error> errorResult)
            return result;

        var error = errorResult.Value;

        var problemDetails = new ProblemDetails()
        {
            Title = "Error",
            Instance = context.HttpContext.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Extensions =
            {
                ["errorCode"] = error!.Code
            }
        };

        logger.LogWarning("Problem details: {problemDetails}", problemDetails);

        return TypedResults.Problem(problemDetails);
    }
}