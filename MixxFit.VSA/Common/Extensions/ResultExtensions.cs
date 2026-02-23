using System.Net;
using MixxFit.VSA.Common.Results;

namespace MixxFit.VSA.Common.Extensions;

public static class ResultExtensions
{
    public static IResult ToTypedResult(this Result result, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (!result.IsSuccess)
            return TypedResults.BadRequest(result.Errors[0]);

        IResult successResult = statusCode switch
        {
            HttpStatusCode.Created => TypedResults.Created(),
            HttpStatusCode.OK => TypedResults.Ok(),
            HttpStatusCode.NoContent => TypedResults.NoContent(),
            _ => TypedResults.Ok()
        };

        return successResult;
    }
}