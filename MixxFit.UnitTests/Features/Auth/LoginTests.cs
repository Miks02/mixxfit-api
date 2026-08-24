using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Domain.Enums;
using MixxFit.API.Domain.ErrorCatalog;
using MixxFit.API.Features.Auth.Login;
using MixxFit.API.Features.Common;
using MixxFit.API.Infrastructure.Security;
using MockQueryable;
using Moq;

namespace MixxFit.UnitTests.Features.Auth;

public class LoginTests
{
    [Fact]
    public async Task Handle_WhenUserIsNull_ShouldReturnFailure()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var emptyUsersList = new List<User>().BuildMock();
        
        userManagerMock
            .Setup(m => m.Users)
            .Returns(emptyUsersList);
        
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginHandler(userManagerMock.Object, tokenServiceMock.Object);

        var loginRequest = new LoginRequest("test123@gmail.com", "123456");

        var result = await handler.Handle(loginRequest);
        
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(AuthError.LoginFailed("Incorrect email or password"));
    }

    [Fact]
    public async Task Handle_WhenPasswordIsIncorrect_ShouldReturnFailure()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var user = new User { Email = "test123@gmail.com" };
        
        var usersList = new List<User> { user }.BuildMock();
        userManagerMock
            .Setup(m => m.Users)
            .Returns(usersList);
        
        userManagerMock
            .Setup(m => m.CheckPasswordAsync(user, "123456"))
            .ReturnsAsync(false);

        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginHandler(userManagerMock.Object, tokenServiceMock.Object);

        var loginRequest = new LoginRequest("test123@gmail.com", "123456");

        var result = await handler.Handle(loginRequest);

        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Should().Be(AuthError.LoginFailed("Incorrect email or password"));
    }

    [Fact]
    public async Task Handle_WhenLoginIsSuccessful_ShouldReturnSuccess()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        var userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginHandler(userManagerMock.Object, tokenServiceMock.Object);
        
        var user = new User
        {
            Id = "user-id-123",
            UserName = "testuser",
            Email = "test123@gmail.com",
            FirstName = "FirstName",
            LastName = "LastName",
            FitnessProfile = new FitnessProfile
            {
                Weight = 80,
                TargetWeight = 75,
                Height = 180,
                DailyCalorieGoal = 2200,
                DateOfBirth = new DateTime(1995, 1, 1),
                Gender = Gender.Male
            }
        };
        
        var userList = new List<User> { user }.BuildMock();
        
        userManagerMock
            .Setup(m => m.Users)
            .Returns(userList);
        
        userManagerMock
            .Setup(m => m.CheckPasswordAsync(user, "123456"))
            .ReturnsAsync(true);
        
        var expectedTokens = new TokenResponseDto("jwt-token", "refresh-token");
        
        tokenServiceMock
            .Setup(t => t.GenerateAuthTokens(user))
            .ReturnsAsync(expectedTokens);
        
        var result = await handler.Handle(new LoginRequest("test123@gmail.com", "123456"));
        
        var userDetails = new UserDetailsDto
        {
            FullName = $"{user.FirstName} {user.LastName}",
            UserName = user.UserName!,
            Email = user.Email!,
            ImagePath = user.ImagePath,
            CurrentWeight = user.FitnessProfile.Weight,
            TargetWeight = user.FitnessProfile.TargetWeight,
            Height = user.FitnessProfile.Height,
            DailyCalorieGoal = user.FitnessProfile.DailyCalorieGoal,
            DateOfBirth = user.FitnessProfile.DateOfBirth,
            AccountStatus = user.AccountStatus,
            Gender = user.FitnessProfile.Gender
        };
        
        var loginResponse = new LoginResponse("jwt-token", "refresh-token", userDetails);

        result.IsSuccess.Should().BeTrue();
        result.Payload.Should().NotBeNull();
        result.Payload.Should().BeEquivalentTo(loginResponse);
    }
}