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

        if (error is null)
            return result;
        
        var status = MapErrorCodeToStatusCode(error.Code);
        var title = GetTitleFromStatusCode(status);

        var problemDetails = new ProblemDetails()
        {
            Title = title,
            Detail = "An error occurred while processing your request.",
            Instance = context.HttpContext.Request.Path,
            Status = status,
            Extensions =
            {
                ["errorCode"] = error.Code
            }
        };

        logger.LogWarning("Problem details: {problemDetails}", problemDetails);

        return TypedResults.Problem(problemDetails);
    }

    private static int MapErrorCodeToStatusCode(string errorCode)
    {
        return errorCode switch
        {
            _ when errorCode.Contains("AlreadyExists") => StatusCodes.Status409Conflict,
            _ when errorCode.Contains("Validation") => StatusCodes.Status400BadRequest,
            _ when errorCode.Contains("LimitReached") => StatusCodes.Status429TooManyRequests,
            _ when errorCode.Contains("Auth") => StatusCodes.Status401Unauthorized,
            _ when errorCode.Contains("Forbidden") => StatusCodes.Status403Forbidden,
            _ when errorCode.Contains("NotFound") => StatusCodes.Status404NotFound,
            _ when errorCode.Contains("Conflict") => StatusCodes.Status409Conflict,
            _ => 500
        };
    }

    private static string GetTitleFromStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Validation",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            429 => "Too Many Requests",
            _ => "Server error occurred"
        };
    }
}