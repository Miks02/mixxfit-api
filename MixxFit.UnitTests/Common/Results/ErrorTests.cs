using AwesomeAssertions;
using MixxFit.API.Common.Results;

namespace MixxFit.UnitTests.Common.Results;

public class ErrorTests
{
    [Fact]
    public void Error_WithCodeAndDescription_ShouldReturnError()
    {
        var error = new Error("Code", "Description");
        
        error.Should().NotBeNull();
        error.Code.Should().Be("Code");
        error.Description.Should().Be("Description");
    }

    [Theory]
    [InlineData("", "description")]
    [InlineData("   ", "description")]
    [InlineData(null, "description")]
    public void Error_WithoutCode_ShouldThrowException(string code, string description)
    {
        Func<Error> act = () => new Error(code, description);
        
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(code))
            .WithMessage("*Error code cannot be empty*");
    }
    
    [Theory]
    [InlineData("code", "")]
    [InlineData("code", "   ")]
    [InlineData("code", null)]
    public void Error_WithoutDescription_ShouldThrowException(string code, string description)
    {
        Func<Error> act = () => new Error(code, description);
        
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(description))
            .WithMessage("*Error description cannot be empty*");
    }
    
    
}