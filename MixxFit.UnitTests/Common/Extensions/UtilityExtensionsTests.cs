using AwesomeAssertions;
using MixxFit.API.Common.Extensions;

namespace MixxFit.UnitTests.Common.Extensions;

public class UtilityExtensionsTests
{
    [Theory]
    [InlineData("TestTest", "testTest")]
    [InlineData("   Test Test   ", "test Test")]
    public void ToLowerFirstLetter_WithValidString_ShouldReturnStringWithALowerCaseFirstLetter(string input, string expected)
    {
        var result = input.ToLowerFirstLetter();
        
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ToLowerFirstLetter_WithNullOrEmptyString_ShouldReturnEmptyString(string str)
    {
        Action act = () => str.ToLowerFirstLetter();
        
        act.Should().Throw<ArgumentException>()
            .WithParameterName(nameof(str))
            .WithMessage("*String cannot be null or empty*");
    }
    
    [Theory]
    [InlineData(15, 60, 960)]
    [InlineData(null, null, null)]
    [InlineData(null, 15, null)]
    [InlineData(15, null, null)]
    public void ToTotalSeconds_ShouldReturnTotalSeconds(int? minutes, int? seconds, int? expected)
    {
        var result = minutes.ToTotalSeconds(seconds);
        
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(960, 16)]
    [InlineData(60, 1)]
    public void ToMinutesFromSeconds_ShouldReturnMinutes(int? seconds, int? expected)
    {
        var result = seconds.ToMinutesFromSeconds();
        
        result.Should().Be(expected);   
    }
    
    [Theory]
    [InlineData(60, 0)]
    [InlineData(75, 15)]
    [InlineData(null, null)]
    public void ToSecondsFromRemainderMinutes_ShouldReturnSeconds(int? minutes, int? expected)
    {
        var result = minutes.ToSecondsFromRemainderMinutes();
        
        result.Should().Be(expected);  
    }
    
    
    
}