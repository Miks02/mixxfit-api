using System.Security.Claims;
using WorkoutTrackerApi.Services.Interfaces;
using WorkoutTrackerApi.Services.Results;

namespace WorkoutTrackerApi.Services;

public class BaseService<T> where T : class
{
    private readonly ILogger<T> _logger;

    public BaseService(ILogger<T> logger)
    {
        _logger = logger; 
    }
    
    protected void LogInformation(string message)
    {
        _logger.LogInformation(message);
    }
    
    protected void LogWarning(string message)
    {
        _logger.LogWarning(message);
    }

    protected void LogError(string message, Exception? ex = null)
    {
        if (ex is not null)
        {
            _logger.LogError(message, ex);
            return;
        }
        
        _logger.LogError(message);
    }

    protected void LogResultErrors(params Error[] errors)
    {
        if (errors.Length == 0)
            throw new ArgumentException("At least one error is required");
        
        foreach (var error in errors )
        {
            _logger.LogError("Code: {code} Description: {description}", error.Code, error.Description);
        }
    }

    protected void LogResultErrors(string message, params Error[] errors)
    {
        _logger.LogError(message);
        
        LogResultErrors(errors);
    }
    
    protected void LogResultErrors(string message, bool isCritical = false, params Error[] errors)
    {
        if(isCritical)
            _logger.LogCritical("CRITICAL CONTEXT: " + message);
        else
            _logger.LogError("CONTEXT: " +message);
        
        
        LogResultErrors(errors);
    }

    protected void LogCritical(string message, Exception? ex = null)
    {
        if (ex is not null)
        {
            _logger.LogError(message, ex);
            return;
        }
        
        _logger.LogCritical(message);
    }
    
}