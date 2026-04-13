using MixxFit.API.Common.Results;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Xunit;


namespace MixxFit.UnitTests.Common.Results;

public class ResultTests
{
    
    // Non-Generic
    
    [Fact]
    public void Success_ShouldReturnSuccessResult()
    {
        var result = Result.Success();
        
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Failure_WithOneError_ShouldReturnFailureResult()
    {
        var result = Result.Failure(new Error("Code", "Description"));
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Code.Should().Be("Code");
    }

    [Fact]
    public void Failure_WithNoErrors_ShouldThrowException()
    {
        Action act = () => Result.Failure(Array.Empty<Error>());
        
        act.Should().Throw<ArgumentException>()
            .WithMessage("At least one error must be provided within a failure");
    }
    
    // Generic - Result<T>

    [Fact]
    public void Success_WithPayload_ShouldReturnSuccessResult()
    {
        var result = Result<string>.Success("Payload");
        
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Payload.Should().Be("Payload");   
    }

    [Fact]
    public void Success_WithoutPayload_ShouldThrowException()
    {
        string? payload = null;
        
        Action act = () => Result<string>.Success(payload!);

        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("payload")
            .WithMessage("*Payload cannot be null*");
        
    }
    
    [Fact]
    public void Failure_WithPayload_ShouldReturnFailureResult()
    {
        var result = Result<string>.Failure(new Error("Code", "Description"));
        
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Code.Should().Be("Code");
        result.Payload.Should().BeNull();
    }

    [Fact]
    public void Failure_WithIdentityErrors_ShouldMapCorrectlyToErrorObject()
    {
        var identityError = new IdentityError
        {
            Code = "PasswordTooShort",
            Description = "Password is required"
        };
        
        var result = Result<string>.Failure(identityError);
        
        result.Errors.Should().ContainSingle();
        result.Errors[0].Code.Should().Be("PasswordTooShort");
        result.Errors[0].Description.Should().Be("Password is required");
    }
}