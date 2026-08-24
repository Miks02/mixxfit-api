using System.Data.Common;
using AwesomeAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using MixxFit.API.Common.Interfaces;
using MixxFit.API.Domain.Entities.FitnessProfiles;
using MixxFit.API.Domain.Entities.Users;
using MixxFit.API.Features.Auth.Register;
using MixxFit.API.Infrastructure.Persistence;
using MixxFit.API.Infrastructure.Security;
using Moq;

namespace MixxFit.UnitTests.Features.Auth;

public class RegisterTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly Mock<IDbContextTransaction> _transactionMock;
    private readonly RegisterHandler _handler;
    private readonly RegisterRequest _request = new()
    {
        Email = "test@gmail.com",
        Password = "StrongPassword123!",
        ConfirmPassword = "StrongPassword123!",
        FirstName = "First",
        LastName = "Last",
        UserName = "username123"
    };

    public RegisterTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );

        var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
            roleStoreMock.Object, null!, null!, null!, null!
        );

        _tokenServiceMock = new Mock<ITokenService>();
        var loggerMock = new Mock<ILogger<RegisterHandler>>();

        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        _dbContextMock = new Mock<AppDbContext>(options);
        _transactionMock = new Mock<IDbContextTransaction>();

        var databaseMock = new Mock<DatabaseFacade>(_dbContextMock.Object);
        databaseMock
            .Setup(d => d.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_transactionMock.Object);

        _dbContextMock.Setup(c => c.Database).Returns(databaseMock.Object);
        _dbContextMock.Setup(c => c.FitnessProfiles).Returns(new Mock<DbSet<FitnessProfile>>().Object);

        _handler = new RegisterHandler(
            _userManagerMock.Object,
            _dbContextMock.Object,
            loggerMock.Object,
            _roleManagerMock.Object,
            _tokenServiceMock.Object
        );
    }

    [Theory]
    [InlineData("PasswordTooShort", "Password is too short.")]
    [InlineData("DuplicateUserName", "Username already exists.")]
    [InlineData("DuplicateEmail", "Email already exists.")]
    public async Task Handle_WhenCreateUserFails_ShouldReturnFailure(string errorCode, string errorDescription)
    {
        _userManagerMock
            .Setup(m => m.CreateAsync(It.IsAny<User>(), _request.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = errorCode, Description = errorDescription }));

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        _roleManagerMock.Verify(r => r.RoleExistsAsync(It.IsAny<string>()), Times.Never);
        _tokenServiceMock.Verify(t => t.GenerateAuthTokens(It.IsAny<User>()), Times.Never);
        _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenRoleDoesNotExistAndRoleCreationFails_ShouldRollbackAndReturnFailure()
    {
        _userManagerMock
            .Setup(m => m.CreateAsync(It.IsAny<User>(), _request.Password))
            .ReturnsAsync(IdentityResult.Success);

        _roleManagerMock
            .Setup(r => r.RoleExistsAsync("User"))
            .ReturnsAsync(false);

        _roleManagerMock
            .Setup(r => r.CreateAsync(It.IsAny<IdentityRole>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "RoleCreationFailed", Description = "Failed" }));

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeFalse();
        _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _tokenServiceMock.Verify(t => t.GenerateAuthTokens(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenAddToRoleFails_ShouldRollbackAndReturnFailure()
    {
        _userManagerMock
            .Setup(m => m.CreateAsync(It.IsAny<User>(), _request.Password))
            .ReturnsAsync(IdentityResult.Success);

        _roleManagerMock
            .Setup(r => r.RoleExistsAsync("User"))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(m => m.AddToRoleAsync(It.IsAny<User>(), "User"))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Code = "AddToRoleFailed", Description = "Failed" }));

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeFalse();
        _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        _tokenServiceMock.Verify(t => t.GenerateAuthTokens(It.IsAny<User>()), Times.Never);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_WhenRegistrationSucceeds_ShouldCommitTransactionAndReturnSuccess(bool roleExists)
    {
        _userManagerMock
            .Setup(m => m.CreateAsync(It.IsAny<User>(), _request.Password))
            .ReturnsAsync(IdentityResult.Success);

        _roleManagerMock
            .Setup(r => r.RoleExistsAsync("User"))
            .ReturnsAsync(roleExists);

        if (!roleExists)
        {
            _roleManagerMock
                .Setup(r => r.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
        }

        _userManagerMock
            .Setup(m => m.AddToRoleAsync(It.IsAny<User>(), "User"))
            .ReturnsAsync(IdentityResult.Success);

        var tokens = new TokenResponseDto("access-token", "refresh-token");
        _tokenServiceMock
            .Setup(t => t.GenerateAuthTokens(It.IsAny<User>()))
            .ReturnsAsync(tokens);

        var result = await _handler.Handle(_request);

        result.IsSuccess.Should().BeTrue();
        result.Payload.Should().NotBeNull();
        result.Payload!.AccessToken.Should().Be(tokens.AccessToken);
        result.Payload.RefreshToken.Should().Be(tokens.RefreshToken);
        result.Payload.User.FullName.Should().Be($"{_request.FirstName} {_request.LastName}");
        result.Payload.User.Email.Should().Be(_request.Email);
        result.Payload.User.UserName.Should().Be(_request.UserName);

        _dbContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDbExceptionOccurs_ShouldRollbackAndRethrow()
    {
        _userManagerMock
            .Setup(m => m.CreateAsync(It.IsAny<User>(), _request.Password))
            .ThrowsAsync(new MockDbException());

        var act = () => _handler.Handle(_request);

        await act.Should().ThrowAsync<DbException>();
        _transactionMock.Verify(t => t.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _transactionMock.Verify(t => t.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private class MockDbException : DbException { }
}

