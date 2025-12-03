using WorkoutTrackerApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using WorkoutTrackerApi.DTO.Auth;
using WorkoutTrackerApi.DTO.User;
using WorkoutTrackerApi.Models;
using WorkoutTrackerApi.Services.Results;

namespace WorkoutTrackerApi.Services.Implementations;

public class AuthService : BaseService<AuthService>,IAuthService
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserService _userService;
    
    public AuthService(
        SignInManager<User> signInManager, 
        UserManager<User> userManager, 
        RoleManager<IdentityRole> roleManager, 
        IUserService userService,
        IHttpContextAccessor http,
        ILogger<AuthService> logger
        ) : base(http, logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _userService = userService;
    }
    
    public async Task<ServiceResult<UserDto>> RegisterAsync(RegisterRequestDto request)
    {
        
        var user = new User()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            Email = request.Email
        };

        var createUser = await _userManager.CreateAsync(user, request.Password);

        if (!createUser.Succeeded)
        {
            LogError("Error occurred while trying to register a new user");

            foreach (var error in createUser.Errors)
            {
                LogError("ERROR: " + error.Description);
                
                if(error.Code == "DuplicateUserName")
                    return ServiceResult<UserDto>.Failure(Error.User.UsernameAlreadyExists());
                if(error.Code == "DuplicateEmail")
                    return ServiceResult<UserDto>.Failure(Error.User.EmailAlreadyExists());
            }
            
            return ServiceResult<UserDto>.Failure(Error.Auth.RegistrationFailed());
        }

        var assignRoleResult = await AssignRoleAsync(user);

        if (!assignRoleResult.IsSucceeded)
        {
            var deleteUserResult = await _userService.DeleteUserAsync(user);

            if (!deleteUserResult.IsSucceeded)
                LogResultErrors("Error occurred while trying to delete user", true, deleteUserResult.Errors.ToArray());
            
            return ServiceResult<UserDto>.Failure(Error.Auth.RegistrationFailed());
        }

        var userDto = new UserDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email
        };
        
        LogInformation("User created successfully");
        return ServiceResult<UserDto>.Success(userDto);
    }

    public async Task<string> LoginAsync(LoginRequestDto request)
    {
        throw new NotImplementedException();
    }

    private async Task<ServiceResult> AssignRoleAsync(User user)
    {
        
        if (!await _roleManager.RoleExistsAsync("User"))
        {
            IdentityResult createRoleResult = await _roleManager.CreateAsync(new IdentityRole("User"));

            if (!createRoleResult.Succeeded)
            {
                var identityErrors = createRoleResult.Errors.Select(e => new Error(e.Code, e.Description));
                
                LogResultErrors("Unexpected error happened while creating a new role for the user", identityErrors.ToArray());
                
                return ServiceResult.Failure(Error.General.UnknownError("Unexpected error happened while creating a new role"));
            }
        }

        IdentityResult addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!addToRoleResult.Succeeded)
        {
            var identityErrors = addToRoleResult.Errors.Select(e => new Error(e.Code, e.Description));

            LogResultErrors("Unexpected error happened while assigning role to the user", identityErrors.ToArray());
            
            return ServiceResult.Failure(Error.General.UnknownError("Unexpected error happened while assigning role to the user"));
        }
        LogInformation("User assigned to role successfully");
        return ServiceResult.Success();

    }
}