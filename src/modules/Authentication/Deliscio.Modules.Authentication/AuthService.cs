using System.Collections.Immutable;
using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Authentication.Common.Errors;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Common.Models.Requests;
using Deliscio.Modules.Authentication.Data.Entities;
using Deliscio.Modules.Authentication.Mappers;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.Authentication;

public sealed class AuthService : IAuthService// SignInManager<AuthUser>, IAuthService
{
    private readonly UserManager<AuthUserEntity> _userManager;
    private readonly RoleManager<AuthRoleEntity> _roleManager;
    private readonly SignInManager<AuthUserEntity> _signInManager;
    private readonly ILogger<AuthService> _logger;

    private const string ERROR_EMAIL_REQUIRED = "Email is required.";

    private const string ERROR_EXCEPTION = "You're Exceptional,";

    private const string ERROR_PASSWORD_REQUIRED = "Password is required.";

    private const string ERROR_USER_EXISTS = "User already exists.";
    private const string ERROR_USERNAME_REQUIRED = "Username is required.";
    private const string ERROR_USER_CANNOT_CREATE = "Could not create the User account.";

    private const string ROLE_ADMIN = "Admin";
    private const string ROLE_USER = "User";
    private const string ROLE_GUEST = "Guest";

    private Role[] _availableRoles = [];

    public AuthService(
        UserManager<AuthUserEntity> userManager,
        RoleManager<AuthRoleEntity> roleManager,
        SignInManager<AuthUserEntity> signInManager,
        ILogger<AuthService> logger)
    {
        Guard.Against.Null(userManager);
        Guard.Against.Null(signInManager);

        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _logger = logger;

        InitRoles();
    }

    private void InitRoles()
    {
        if (!_availableRoles.Any())
        {
            var authRoles = _roleManager.Roles.ToArray();

            _availableRoles = Mapper.Map(authRoles)?.ToArray() ?? [];
        }
    }

    public async ValueTask<Result<bool>> AddUserToRoleAsync(string userId, string roleId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Fail<bool>("User ID is required.");

        if (string.IsNullOrWhiteSpace(roleId))
            return Result.Fail<bool>("Role ID is required.");

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Fail<bool>("User not found.");

        var role = await _roleManager.FindByIdAsync(roleId);

        if (role is null)
            return Result.Fail<bool>("Role not found.");

        var result = await _userManager.AddToRoleAsync(user, role.Name);

        if (!result.Succeeded)
            return Result.Fail<bool>(result.Errors?.Select(e => e.Description).ToArray() ?? Array.Empty<string>());

        return Result.Ok(true);
    }

    /// <summary>
    /// Creates a new Auth User.
    /// This does not include the creation of a Profile.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async ValueTask<Result<User?>> CreateUserAsync(CreateAuthUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return Result.Fail(ERROR_USERNAME_REQUIRED);

        if (string.IsNullOrWhiteSpace(request.Email))
            return Result.Fail(ERROR_EMAIL_REQUIRED);

        if (string.IsNullOrWhiteSpace(request.Password))
            return Result.Fail(ERROR_PASSWORD_REQUIRED);

        try
        {
            var userRslt = await DoesUserExistAsync(request.Username, request.Email);

            if (userRslt.DoesExist)
                return Result.Fail(ERROR_USER_EXISTS);

            var authUser = new AuthUserEntity { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(authUser, request.Password);

            if (!result.Succeeded)
            {
                // If there were any error messages, then get the description(s) and return to the authUser
                var errors = result.Errors?.AsEnumerable().Select(e => e.Description).ToArray() ?? Array.Empty<string>();

                return Result.Fail(errors);
            }

            var user = Mapper.Map(authUser)!;

            if (request.RoleIds.Any())
            {
                var usersRoles = new List<string>();

                foreach (var roleId in request.RoleIds)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);

                    if (role is not null)
                    {
                        var didRoleSave = await _userManager.AddToRoleAsync(authUser, role.Name!);

                        if (didRoleSave.Succeeded)
                        {
                            usersRoles.Add(role.Name!);
                        }
                    }
                }

                user.Roles = usersRoles.ToArray();
            }

            return Result.Ok<User?>(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, ERROR_EXCEPTION);

            return Result.Fail(e.Message);
        }
    }

    public Result<PagedResults<User>> GetUsersAsync(int pageNo = 1, int pageSize = 50)
    {
        var skip = (pageNo - 1) * pageSize;
        var take = pageSize;

        var usersQueryable = _userManager.Users;

        var count = usersQueryable.Count();

        var authUsers = _userManager.Users?.Skip(skip).Take(take).ToArray() ?? [];

        List<User> users = new();

        if (authUsers.Any())
        {
            foreach (var authUser in authUsers)
            {
                var user = Mapper.Map(authUser);

                if (user is not null)
                {

                    if (authUser.Roles.Any())
                    {
                        var userRoles = (_availableRoles
                            .Where(r => authUser.Roles
                                .Select(u => u.ToString()).ToList()
                                .Contains(r.Id))
                            .Select(u => u.Name)
                            .ToArray());

                        if (userRoles.Any())
                        {
                            user.Roles = userRoles;
                        }
                    }

                    users.Add(user);
                }

            }
        }

        var page = new PagedResults<User>(users, pageNo, pageSize, count);

        return Result.Ok(page);
    }

    public async Task<Result<User>> GetUserAsync(string id)
    {
        var authUser = await _userManager.FindByIdAsync(id);

        if (authUser is null)
            return Result.Fail<User>(new UserNotFoundError());

        var user = Mapper.Map(authUser);

        user.Roles = (await _userManager.GetRolesAsync(authUser))?.ToArray() ?? [];

        return user!;
    }

    public async Task<Result<User>> GetUserByEmailAsync(string email)
    {
        var authUser = await _userManager.FindByEmailAsync(email);

        var user = Mapper.Map(authUser);

        return user;
    }

    public async Task<Result<User>> GetUserByNameAsync(string name)
    {
        var authUser = await _userManager.FindByNameAsync(name);

        var user = Mapper.Map(authUser);

        return user;
    }

    /// <summary>
    /// Attempts to register a authUser.
    /// This only adds a AuthUser account, Profile is done separately.
    /// </summary>
    /// <param name="username">The authUser's username</param>
    /// <param name="email">The authUser's email address</param>
    /// <param name="password">The authUser's password</param>
    /// <returns>
    /// A tuple with a value indicating whether or not the registration was a success.
    /// If the registration failed for some reason, an array or error messages is populated
    /// </returns>
    public async ValueTask<Result<AuthUserEntity?>> RegisterUserAsync(string username, string email, string password, bool isPersistent)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Result.Fail(ERROR_USERNAME_REQUIRED);

        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail("Email is required");

        if (string.IsNullOrWhiteSpace(password))
            return Result.Fail("Password is required");

        try
        {
            var rslt = await DoesUserExistAsync(username, email);

            if (rslt.DoesExist)
                return Result.Fail(ERROR_USER_EXISTS);

            var user = new AuthUserEntity { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // If there were any error messages, then get the description(s) and return to the authUser
                var errors = result.Errors?.AsEnumerable().Select(e => e.Description).ToArray() ?? Array.Empty<string>();

                return Result.Fail(errors);
            }

            await _userManager.AddToRoleAsync(user, ROLE_USER);

            await _signInManager.SignInAsync(user, isPersistent: true);

            return Result.Ok<AuthUserEntity?>(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, ERROR_EXCEPTION);

            return Result.Fail(e.Message);
        }
    }

    public async Task<Result<SignInResult>> SignInAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login))
            return Result.Fail("Login is required");

        if (string.IsNullOrWhiteSpace(request.Password))
            return Result.Fail("Password is required");

        //var user = await _userManager.FindByEmailAsync(request.Login);

        //if (user == null)
        //    user = await _userManager.FindByNameAsync(request.Login);

        //if (user == null)
        //    return Result.Fail("User not found");

        var isPersistent = true;
        var lockoutOnFailure = false;

        var result = await _signInManager.PasswordSignInAsync(request.Login, request.Password, isPersistent, lockoutOnFailure);

        // If the previous fails, then IF the login looks like an email address, try to find the user by email and then use their username to login
        if (!result.Succeeded && request.Login.Contains('@'))
        {
            var user = await _userManager.FindByEmailAsync(request.Login);

            if (user != null)
            {
                result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, isPersistent, lockoutOnFailure);
            }
        }

        return Result.Ok(result);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async ValueTask<Result<Role?>> CreateRoleAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Fail<Role?>("Role name is required.");

        var existingRole = await _roleManager.FindByNameAsync(name);

        if (existingRole is not null)
            return Result.Fail<Role?>("Role already exists.");

        var role = new AuthRoleEntity { Name = name };
        var rslt = await _roleManager.CreateAsync(role);

        if (!rslt.Succeeded)
            return Result.Fail<Role?>(rslt.Errors?.Select(e => e.Description).ToArray() ?? Array.Empty<string>());

        return Result.Ok(Mapper.Map(role));
    }

    public ValueTask<Result<Role?>> DeleteRoleAsync(string id, string name)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Result<Role[]>> GetRolesAsync()
    {
        if (!_availableRoles.Any())
        {
            var authRoles = _roleManager.Roles.ToArray();

            if (!authRoles.Any())
            {
                // Create the default roles - too lazy to do this in a migration at the moment
                await CreateRoleAsync("Admin");
                await CreateRoleAsync("User");

                authRoles = _roleManager.Roles.ToArray();
            }

            _availableRoles = Mapper.Map(authRoles)?.ToArray() ?? [];
        }

        return Result.Ok(_availableRoles);
    }

    public Result<Role?> GetRoleAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result.Fail<Role?>("Role ID is required.");

        if (!_availableRoles.Any())
            return Result.Fail<Role?>("No roles available.");

        return _availableRoles.FirstOrDefault(r => r.Id.Equals(id, StringComparison.CurrentCultureIgnoreCase));
    }

    public Result<Role?> GetRoleByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Fail<Role?>("Role name is required.");

        if (!_availableRoles.Any())
            return Result.Fail<Role?>("No roles available.");

        return _availableRoles.FirstOrDefault(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }

    public Result<Role[]> GetRolesAsync(string[] ids)
    {
        if (!ids.Any())
            return Result.Ok(Array.Empty<Role>());

        var roles = new List<Role>();

        foreach (var id in ids)
        {
            var role = GetRoleAsync(id);

            if (role is { IsSuccess: true, ValueOrDefault: not null })
            {
                roles.Add(role.Value!);
            }
        }

        return Result.Ok(roles.ToArray());
    }

    public Task<IdentityResult> ValidateAsync(UserManager<AuthUserEntity> manager, AuthUserEntity authUser)
    {


        return Task.FromResult(IdentityResult.Success);
    }



    private async Task<(bool DoesExist, AuthUserEntity? User)> DoesUserExistAsync(string username, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null)
            return (true, user);

        user = await _userManager.FindByNameAsync(username);

        if (user != null)
            return (true, user);

        return (false, default);
    }
}

