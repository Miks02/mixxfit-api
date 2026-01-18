using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VitalOps.API.Services.Results;
using VitalOps.API.DTO.Global;

namespace VitalOps.API.Extensions;

public static class ServiceResultExtensions
{
    public static ServiceResult HandleResult(this ServiceResult result, ILogger? logger)
    {
        if (result.IsSucceeded)
            return ServiceResult.Success();

        LogErrors(logger, result.Errors);
        return ServiceResult.Failure(result.Errors.ToArray());
    }

    public static ServiceResult HandleResult<T>(this ServiceResult<T> result, ILogger? logger)
    {
        if (result.IsSucceeded)
            return result.Payload is null ? ServiceResult<T>.Success() : ServiceResult<T>.Success(result.Payload);

        LogErrors(logger, result.Errors);
        return ServiceResult.Failure(result.Errors.ToArray());
    }

    public static ServiceResult HandleIdentityResult(this IdentityResult result, ILogger? logger = null)
    {
        if (result.Succeeded)
            return ServiceResult.Success();

        var errors = ConvertToErrorList(result.Errors);

        LogErrors(logger, errors, "Identity operation failed");
        return ServiceResult.Failure(errors.ToArray());
    }

    public static ServiceResult<T> HandleIdentityResult<T>(this IdentityResult result, T? data, ILogger? logger = null)
    {
        if (result.Succeeded)
            return data is null ? ServiceResult<T>.Success() : ServiceResult<T>.Success(data);

        var errors = ConvertToErrorList(result.Errors);

        LogErrors(logger, errors, "Identity operation failed");
        return ServiceResult<T>.Failure(errors.ToArray());
    }

    public static ActionResult ToActionResult(this ServiceResult result)
    {
        if (result.IsSucceeded)
            return new NoContentResult();
        
        return new ObjectResult(result.Errors[0]) {StatusCode = 400};
    }

    public static ActionResult ToActionResult<T>(this ServiceResult<T> result)
    {
        if (result.IsSucceeded)
            return new OkObjectResult(result.Payload);

        return new ObjectResult(result.Errors[0]) { StatusCode = 400 };
    }

    private static void LogErrors(ILogger? logger, IReadOnlyList<Error> errors, string message = "Operation failed")
    {
        if (logger is null)
            return;

        logger.LogError(message);
        foreach (var error in errors)
            logger.LogWarning("ERROR: {code} | {description}", error.Code, error.Description);
    }

    private static Error MapIdentityError(IdentityError error)
    {
        return error.Code switch
        {
            "DuplicateUserName" => Error.User.UsernameAlreadyExists(),
            "InvalidUserName" => Error.Validation.InvalidInput($"Provided username is not valid {error.Description}"),
            "DuplicateEmail" => Error.User.EmailAlreadyExists(),
            "InvalidEmail" => Error.Validation.InvalidInput($"Provided email address is not valid {error.Description}"),
            _ => new Error(error.Code, error.Description)
        };
    }

    private static IReadOnlyList<Error> ConvertToErrorList(IEnumerable<IdentityError> errors)
    {
        return errors.Select(MapIdentityError).ToList();
    }


}