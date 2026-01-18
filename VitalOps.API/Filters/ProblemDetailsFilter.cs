using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VitalOps.API.DTO.Global;
using Error = VitalOps.API.Services.Results.Error;
using Results_Error = VitalOps.API.Services.Results.Error;

namespace VitalOps.API.Filters;

public class ProblemDetailsFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {

        if (context.Result is not ObjectResult objectResult)
            return;

        if (objectResult.Value is not Results_Error error)
            return;

        if (objectResult.StatusCode == 200)
            return;

        Console.WriteLine("Object result value: " + objectResult.Value);

        var problemDetails = CreateProblemDetails(error, context.HttpContext);

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

    }

    public void OnResultExecuted(ResultExecutedContext context)
    {

    }

    private int MapErrorCodeToStatusCode(string errorCode)
    {
        return errorCode switch
        {
            var code when code.Contains("AlreadyExists") => 409,
            var code when code.Contains("Validation") => 400,
            var code when code.Contains("Auth") => 401,
            var code when code.Contains("Forbidden") => 403,
            var code when code.Contains("NotFound") => 404,
            var code when code.Contains("Conflict") => 409,
            _ => 500
        };
    }

    private string GetTitleFromStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Validation",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            _ => "Server error occurred"
        };
    }

    private ProblemDetails CreateProblemDetails(Results_Error error, HttpContext context)
    {
        var statusCode = MapErrorCodeToStatusCode(error.Code);
        var title = GetTitleFromStatusCode(statusCode);

        return new ProblemDetails()
        {
            Status = statusCode,
            Title = title,
            Detail = error.Description,
            Instance = context.Request.Path,
            Extensions =
            {
                ["errorCode"] = error.Code
            }
        };

    }
}
