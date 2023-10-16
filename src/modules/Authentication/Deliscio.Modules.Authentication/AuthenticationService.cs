using Ardalis.GuardClauses;
using Deliscio.Modules.Authentication.Common.Interfaces;
using Deliscio.Modules.Authentication.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.Authentication;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly SignInManager<AuthUser> _signInManager;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(UserManager<AuthUser> userManager, SignInManager<AuthUser> signInManager,
        ILogger<AuthenticationService> logger)
    {
        Guard.Against.Null(userManager);
        Guard.Against.Null(signInManager);

        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

}
