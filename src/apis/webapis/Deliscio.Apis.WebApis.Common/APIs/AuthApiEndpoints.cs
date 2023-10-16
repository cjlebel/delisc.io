using System.Net;
using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Modules.Authentication.Common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Deliscio.Apis.WebApi.Common.APIs;

public class AuthApiEndpoints : BaseApiEndpoints
{
    private readonly ILogger<LinksApiEndpoints> _logger;
    private readonly SignInManager<AuthUser> _signInManager;
    private readonly UserManager<AuthUser> _userManager;

    public AuthApiEndpoints(SignInManager<AuthUser> signInManager, UserManager<AuthUser> userManager, ILogger<LinksApiEndpoints> logger)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        MapPostSignIn(endpoints);
    }

    private void MapPostSignIn(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/v1/account/signin", async context =>
        {
            var request = await context.Request.ReadFromJsonAsync<SignInRequest>();

            if (request is null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            var user = await _userManager.FindByEmailAsync(request.EmailOrUserName);

            if (user == null)
            {
                user = await _userManager.FindByNameAsync(request.EmailOrUserName);
            }

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var isPersistent = true; // Set this based on your requirements
            await _signInManager.SignInAsync(user, isPersistent);

            await context.Response.WriteAsJsonAsync("Successfully authenticated!");
        });

        endpoints.MapPost("/v1/account/signout", async context =>
        {
            await _signInManager.SignOutAsync();

            await context.Response.WriteAsJsonAsync("Successfully signed out!");
        });

        endpoints.MapPost("/v1/account/register", async context =>
        {
            var request = await context.Request.ReadFromJsonAsync<RegisterRequest>();

            if (request is null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync("Invalid request.");

                return;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync("Email is already taken.");

                return;
            }

            user = new AuthUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync("There was a problem creating your account.");

                return;
            }

            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, isPersistent: false);

            await context.Response.WriteAsJsonAsync("Successfully registered!");
        });
    }
}