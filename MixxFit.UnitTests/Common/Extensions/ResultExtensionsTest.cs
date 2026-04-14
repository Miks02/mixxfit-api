using System.Net;
using AwesomeAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using MixxFit.API.Common.Extensions;
using MixxFit.API.Common.Results;

namespace MixxFit.UnitTests.Common.Extensions;

public class ResultExtensionsTest
{
    [Fact]
    public void ToTypedResult_WithSuccessfulResult_AndDefaultStatusCode_ShouldReturnOk()
    {
        var result = Result.Success();

        var typedResult = result.ToTypedResult();

        typedResult.Should().BeOfType<Ok>();
    }

    [Fact]
    public void ToTypedResult_WithSuccessfulResult_AndCreatedStatusCode_ShouldReturnCreated()
    {
        var result = Result.Success();

        var typedResult = result.ToTypedResult(HttpStatusCode.Created);

        typedResult.Should().BeOfType<Created>();
    }

    [Fact]
    public void ToTypedResult_WithSuccessfulResult_AndNoContentStatusCode_ShouldReturnNoContent()
    {
        var result = Result.Success();

        var typedResult = result.ToTypedResult(HttpStatusCode.NoContent);

        typedResult.Should().BeOfType<NoContent>();
    }

    [Fact]
    public void ToTypedResult_WithFailedResult_ShouldReturnBadRequest()
    {
        var result = Result.Failure(new Error("Code", "Description"));

        var typedResult = result.ToTypedResult();

        typedResult.Should().BeOfType<BadRequest<Error>>();
    }

    [Fact]
    public void ToTypedResult_Generic_WithSuccessfulResult_AndDefaultStatusCode_ShouldReturnOkOfT()
    {
        var result = Result<string>.Success("Payload");

        var typedResult = result.ToTypedResult();

        typedResult.Should().BeOfType<Ok<string>>();
    }

    [Fact]
    public void ToTypedResult_Generic_WithSuccessfulResult_AndCreatedStatusCode_ShouldReturnCreatedOfT()
    {
        var result = Result<string>.Success("Payload");

        var typedResult = result.ToTypedResult(HttpStatusCode.Created, "/api/users/1");

        typedResult.Should().BeOfType<Created<string>>();
    }

    [Fact]
    public void ToTypedResult_Generic_WithSuccessfulResult_AndNoContentStatusCode_ShouldReturnNoContent()
    {
        var result = Result<string>.Success("Payload");

        var typedResult = result.ToTypedResult(HttpStatusCode.NoContent);

        typedResult.Should().BeOfType<NoContent>();
    }

    [Fact]
    public void ToTypedResult_Generic_WithFailedResult_ShouldReturnBadRequest()
    {
        var result = Result<string>.Failure(new Error("Code", "Description"));

        var typedResult = result.ToTypedResult();

        typedResult.Should().BeOfType<BadRequest<Error>>();
    }

    [Fact]
    public void HandleResult_WithSuccessResult_ShouldReturnSuccessResult()
    {
        var result = Result.Success();

        var handledResult = result.HandleResult();

        handledResult.IsSuccess.Should().BeTrue();
        handledResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void HandleResult_WithFailureResult_ShouldReturnFailureResultWithSameErrors()
    {
        var result = Result.Failure(new Error("Code", "Description"));

        var handledResult = result.HandleResult();

        handledResult.IsSuccess.Should().BeFalse();
        handledResult.Errors.Should().ContainSingle();
        handledResult.Errors[0].Code.Should().Be("Code");
        handledResult.Errors[0].Description.Should().Be("Description");
    }

    [Fact]
    public void HandleResult_Generic_WithSuccessResultAndPayload_ShouldReturnSuccessResultWithPayload()
    {
        var result = Result<string>.Success("Payload");

        var handledResult = result.HandleResult();

        handledResult.IsSuccess.Should().BeTrue();
        handledResult.Payload.Should().Be("Payload");
        handledResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void HandleResult_Generic_WithSuccessResultWithoutPayload_ShouldReturnSuccessResultWithoutPayload()
    {
        var result = Result<string>.Success();

        var handledResult = result.HandleResult();

        handledResult.IsSuccess.Should().BeTrue();
        handledResult.Payload.Should().BeNull();
        handledResult.Errors.Should().BeEmpty();
    }

    [Fact]
    public void HandleResult_Generic_WithFailureResult_ShouldReturnFailureResultWithSameErrors()
    {
        var result = Result<string>.Failure(new Error("Code", "Description"));

        var handledResult = result.HandleResult();

        handledResult.IsSuccess.Should().BeFalse();
        handledResult.Payload.Should().BeNull();
        handledResult.Errors.Should().ContainSingle();
        handledResult.Errors[0].Code.Should().Be("Code");
        handledResult.Errors[0].Description.Should().Be("Description");
    }

    [Fact]
    public void HandleIdentityResult_WithSucceededIdentityResult_ShouldReturnSuccessResult()
    {
        var identityResult = IdentityResult.Success;

        var result = identityResult.HandleIdentityResult();

        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void HandleIdentityResult_WithFailedIdentityResult_ShouldReturnFailureResult()
    {
        var identityResult = IdentityResult.Failed(new IdentityError
        {
            Code = "SomeRandomCode",
            Description = "Random description"
        });

        var result = identityResult.HandleIdentityResult();

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Code.Should().Be("SomeRandomCode");
        result.Errors[0].Description.Should().Be("Random description");
    }

    [Fact]
    public void HandleIdentityResult_Generic_WithSucceededIdentityResult_ShouldReturnSuccessResultWithPayload()
    {
        var identityResult = IdentityResult.Success;

        var result = identityResult.HandleIdentityResult("Payload");

        result.IsSuccess.Should().BeTrue();
        result.Payload.Should().Be("Payload");
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void HandleIdentityResult_Generic_WithFailedIdentityResult_ShouldReturnFailureResult()
    {
        var identityResult = IdentityResult.Failed(new IdentityError
        {
            Code = "SomeRandomCode",
            Description = "Random description"
        });

        var result = identityResult.HandleIdentityResult("Payload");

        result.IsSuccess.Should().BeFalse();
        result.Payload.Should().BeNull();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Code.Should().Be("SomeRandomCode");
        result.Errors[0].Description.Should().Be("Random description");
    }

    [Theory]
    [InlineData("DuplicateUserName", "User.UsernameAlreadyExists")]
    [InlineData("DuplicateEmail", "User.EmailAlreadyExists")]
    [InlineData("PasswordMismatch", "Auth.InvalidCurrentPassword")]
    [InlineData("PasswordTooShort", "Auth.PasswordTooShort")]
    [InlineData("PasswordRequiresDigit", "Auth.PasswordRequiresDigit")]
    [InlineData("PasswordRequiresUpper", "Auth.PasswordRequiresUpper")]
    [InlineData("PasswordRequiresNonAlphanumeric", "Auth.PasswordRequiresNonAlphanumeric")]
    [InlineData("SomeRandomCode", "SomeRandomCode")]
    public void HandleIdentityResult_WhenFailed_ShouldMapCorrectErrorCode(string identityCode, string expectedErrorCode)
    {
        var identityError = new IdentityError
        {
            Code = identityCode,
            Description = "Test description"
        };
        var identityResult = IdentityResult.Failed(identityError);

        var result = identityResult.HandleIdentityResult();

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().ContainSingle();
        result.Errors[0].Code.Should().Be(expectedErrorCode);
    }
}