using Ardalis.GuardClauses;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.Authentication;

public sealed class AuthService : IAuthService //SignInManager<AuthUser>, IAuthService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;
    private readonly ILogger<AuthService> _logger;

    private const string ERROR_CANNOT_REGISTER_EMAIL = "Cannot register with this email.";
    private const string ERROR_EXCEPTION = "You're Exceptional,";
    private const string ERROR_USERNAME_TAKEN = "The username is already spoekn for.";
    private const string ERROR_CANNOT_CREATE_USER = "Could not create the authUser account.";

    private const string ROLE_ADMIN = "Admin";
    private const string ROLE_USER = "AuthUser";

    public AuthService(
        UserManager<AuthUser> userManager,
        SignInManager<AuthUser> signInManager,
        ILogger<AuthService> logger)
    {
        Guard.Against.Null(userManager);
        Guard.Against.Null(signInManager);

        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IEnumerable<AuthUser>> GetUsers()
    {
        return _userManager.Users.ToList();
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
    public async Task<(bool IsSuccess, string Message, string[] ErrorMessages)> RegisterAsync(string username, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return (false, "Failure", new[] { "Username is required" });

        if (string.IsNullOrWhiteSpace(email))
            return (false, "Failure", new[] { "Email is required" });

        if (string.IsNullOrWhiteSpace(password))
            return (false, "Failure", new[] { "Password is required" });

        var errors = Array.Empty<string>();

        AuthUser? user;

        try
        {
            user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                errors = new[] { ERROR_CANNOT_REGISTER_EMAIL };

                return (false, "Failure", errors);
            }

            user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                errors = new[] { ERROR_USERNAME_TAKEN };

                return (false, "Failure", errors);
            }

            user = new AuthUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // If there were any error messages, then get the description(s) and return to the authUser
                errors = result.Errors?.AsEnumerable().Select(e => e.Description).ToArray() ?? Array.Empty<string>();

                return (false, "Failure", errors);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            errors = new[] { e.Message };

            return (false, ERROR_EXCEPTION, errors);
        }



        await _userManager.AddToRoleAsync(user, ROLE_USER);

        await _signInManager.SignInAsync(user, isPersistent: true);

        return (true, "Success", errors);
    }

    public async Task<(bool IsSuccess, string Message, AuthUser? user)> SignInAsync(string emailOrUserName, string password)
    {
        Guard.Against.NullOrEmpty(emailOrUserName);
        Guard.Against.NullOrEmpty(password);

        var user = await _userManager.FindByEmailAsync(emailOrUserName);

        if (user == null)
            user = await _userManager.FindByNameAsync(emailOrUserName);

        if (user == null)
            return (false, "AuthUser not found", null);

        var isPersistent = true;
        var lockoutOnFailure = false;

        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);

        if (!result.Succeeded)
            return (false, "Login failed", null);

        return (true, "Login succeeded", user);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }



    public Task<IdentityResult> ValidateAsync(UserManager<AuthUser> manager, AuthUser authUser)
    {


        return Task.FromResult(IdentityResult.Success);
    }
}

