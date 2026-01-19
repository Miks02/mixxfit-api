using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VitalOps.API.DTO.Auth;
using VitalOps.API.Models;
using VitalOps.API.Services.Interfaces;
using VitalOps.API.Services.Results;

namespace VitalOps.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthService
            (
            ILogger<AuthService> logger,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IUserService userService
            )
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _userService = userService;
    }


    public async Task<Result<AuthResponseDto>> RegisterAsync(
        RegisterRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email,
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            _logger.LogError("Error happened while trying to create a user");

            foreach (var error in createResult.Errors)
            {
                _logger.LogError("ERROR: {error}", error.Description);
                if (error.Code == "DuplicateEmail")
                    return Result<AuthResponseDto>.Failure(Error.User.EmailAlreadyExists());
                if (error.Code == "DuplicateUserName")
                    return Result<AuthResponseDto>.Failure(Error.User.UsernameAlreadyExists());
            }
        }

        var assignResult = await AssignRoleAsync(user);

        if (!assignResult.IsSucceeded)
            return Result<AuthResponseDto>.Failure(assignResult.Errors.ToArray());

        var generateTokens = await GenerateAuthTokens(user);

        if (!generateTokens.IsSucceeded)
        {
            _logger.LogError("Failed to generate tokens for a newly registered user {id}", user.Id);
            return Result<AuthResponseDto>.Failure(generateTokens.Errors.ToArray());
        }

        var userDetails = await _userService.GetUserDetailsAsync(user.Id, cancellationToken);

        var responseDto = new AuthResponseDto()
        {
            AccessToken = generateTokens.Payload!.AccessToken,
            RefreshToken = generateTokens.Payload!.RefreshToken,
            User = userDetails
        };

        return Result<AuthResponseDto>.Success(responseDto);


    }

    public async Task<Result<AuthResponseDto>> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            _logger.LogError($"Failed sign in for user with email: {request.Email}. User not found");
            return Result<AuthResponseDto>.Failure(Error.Auth.LoginFailed("Incorrect email or password"));
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            _logger.LogError($"Failed sign in for user with email: {request.Email}. Incorrect password");
            return Result<AuthResponseDto>.Failure(Error.Auth.LoginFailed("Incorrect email or password"));
        }

        var newAccessToken = await GenerateAuthTokens(user);

        if(!newAccessToken.IsSucceeded)
            return Result<AuthResponseDto>.Failure(newAccessToken.Errors.ToArray());

        var userDetails = await _userService.GetUserDetailsAsync(user.Id, cancellationToken);

        var responseDto = new AuthResponseDto()
        {
            AccessToken = newAccessToken.Payload!.AccessToken,
            RefreshToken = newAccessToken.Payload!.RefreshToken,
            User = userDetails
        };
        

        return Result<AuthResponseDto>.Success(responseDto);
    }

    public async Task<Result<AuthResponseDto>> RotateAuthTokens(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Where(u => u.RefreshToken == refreshToken)
            .FirstOrDefaultAsync(cancellationToken);

        _logger.LogInformation("Refresh token value: {refreshToken}", refreshToken);

        if (user is null)
        {
            _logger.LogError("Failed to regenerate access and refresh tokens. User is null");
            return Result<AuthResponseDto>.Failure(Error.Auth.JwtError("Failed to regenerate auth tokens"));
        }

        if (user.RefreshToken is null)
        {
            _logger.LogError("Failed to regenerate access and refresh tokens. Refresh token is null");
            return Result<AuthResponseDto>.Failure(Error.Auth.JwtError("Failed to regenerate auth tokens"));
        }

        if (user.TokenExpDate < DateTime.UtcNow)
        {
            var error = Error.Auth.ExpiredToken();
            _logger.LogError("Failed to regenerate auth tokens. " + error.Description);
            return Result<AuthResponseDto>.Failure(error);
        }

        var newAccessToken = await CreateAccessToken(user);
        var newRefreshToken = await AssignRefreshToken(user);

        if (!newRefreshToken.IsSucceeded)
        {
            foreach (var error in newRefreshToken.Errors)
            {
                _logger.LogError("Code: {code} Description: {description}", error.Code, error.Description);
            }
            return Result<AuthResponseDto>.Failure(newRefreshToken.Errors.ToArray());
        }

        var authResponse = new AuthResponseDto()
        {
            RefreshToken = newRefreshToken.Payload!,
            AccessToken = newAccessToken
        };

        return Result<AuthResponseDto>.Success(authResponse);

    }

    public async Task<Result> LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new InvalidOperationException("Refresh token is missing");
        }
        
        var user = await _userManager.Users
            .Where(u => u.RefreshToken == refreshToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
        {
            _logger.LogError("User with refresh token {refreshToken} has not been found", refreshToken);
            return Result.Failure(Error.User.NotFound());
        }

        user.RefreshToken = null;
        user.TokenExpDate = null;

        var removeTokenResult = await _userManager.UpdateAsync(user);

        if (!removeTokenResult.Succeeded)
        {

            var errors = removeTokenResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
  
            _logger.LogError("Failed to sign out | UserID: {id}", user.Id);
            foreach (var error in errors)
            {
                _logger.LogError("Code: {code} Description: {description}", error.Code, error.Description);
            }
            return Result.Failure(errors.ToArray());
        }

        return Result.Success();
    }

    private async Task<string> CreateAccessToken(User user)
    {
        var rolesList = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        claims.AddRange(rolesList.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Token"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            issuer: _configuration["JwtConfig:Issuer"],
            audience: _configuration["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetValue<double>("JwtConfig:ExpirationInMinutes")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string CreateRefreshToken()
    {
        var randomBytes = new Byte[32];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    private async Task<Result<string>> AssignRefreshToken(User user)
    {
        user.RefreshToken = CreateRefreshToken();
        user.TokenExpDate = DateTime.UtcNow.AddDays(7);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogError("Error happened while assigning refresh token to the user");
            return Result<string>.Failure(Error.Auth.JwtError());
        }

        return Result<string>.Success(user.RefreshToken);
    }

    private async Task<Result<TokenResponseDto>> GenerateAuthTokens(User user)
    {

        var assignRefreshToken = await AssignRefreshToken(user);

        if (!assignRefreshToken.IsSucceeded)
            return Result<TokenResponseDto>.Failure(assignRefreshToken.Errors.ToArray());


        var tokenResponse = new TokenResponseDto()
        {
            AccessToken = await CreateAccessToken(user),
            RefreshToken = assignRefreshToken.Payload!
        };

        _logger.LogInformation("Auth tokens generated successfully");
        return Result<TokenResponseDto>.Success(tokenResponse);

    }

    private async Task<Result> AssignRoleAsync(User user)
    {

        if (!await _roleManager.RoleExistsAsync("User"))
        {
            IdentityResult createRoleResult = await _roleManager.CreateAsync(new IdentityRole("User"));

            if (!createRoleResult.Succeeded)
            {
                var identityErrors = createRoleResult.Errors.Select(e => new Error(e.Code, e.Description));

                _logger.LogError("Unexpected error happened while creating a new role for the user");
                foreach (var error in identityErrors)
                {
                    _logger.LogError("Code: {code} Description: {description}", error.Code, error.Description);
                }

                return Result.Failure(Error.General.UnknownError("Unexpected error happened while creating a new role"));
            }
        }

        IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!addToRoleResult.Succeeded)
        {
            var identityErrors = addToRoleResult.Errors.Select(e => new Error(e.Code, e.Description));

            _logger.LogError("Unexpected error happened while assigning role to the user");
            foreach (var error in identityErrors)
            {
                _logger.LogError("Code: {code} Description: {description}", error.Code, error.Description);
            }

            return Result.Failure(Error.General.UnknownError("Unexpected error happened while assigning role to the user"));
        }
        _logger.LogInformation("User assigned to role successfully");
        return Result.Success();

    }
}
