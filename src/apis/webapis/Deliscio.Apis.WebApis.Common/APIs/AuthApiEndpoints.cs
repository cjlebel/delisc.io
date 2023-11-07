using System.Net;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Common.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Deliscio.Apis.WebApi.Common.APIs;

//public class AuthApiEndpoints : BaseApiEndpoints
//{
//    private readonly IAuthenticationManager _authManager;
//    private readonly ILogger<LinksApiEndpoints> _logger;

//    public AuthApiEndpoints(IAuthenticationManager authManager, ILogger<LinksApiEndpoints> logger)
//    {
//        _authManager = authManager;
//        _logger = logger;
//    }

//    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
//    {
//        MapPostSignIn(endpoints);
//    }

//    private static void MapPostSignIn(IEndpointRouteBuilder endpoints)
//    {
//        endpoints.MapPost("/v1/auth/signin", [AllowAnonymous] async ([FromBody] SignInRequest signinRequest, IAuthorizationService authService, HttpRequest request, HttpResponse response) =>
//        {

//        });
//    }

//    ///// <summary>
//    ///// Maps the POST endpoint for registering.
//    ///// </summary>
//    ///// <param name="endpoints"></param>
//    //private void MapPostRegister(IEndpointRouteBuilder endpoints)
//    //{
//    //    endpoints.MapPost("/v1/auth/register", async context =>
//    //    {
//    //        using (var scope = context.RequestServices.CreateScope())
//    //        {
//    //            var authEndpoints = scope.ServiceProvider.GetRequiredService<AuthApiEndpoints>();
//    //        }
//    //        var request = await context.Request.ReadFromJsonAsync<RegisterRequest>();

//    //        if (request is null)
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//    //            await context.Response.WriteAsJsonAsync("Invalid request.");

//    //            return;
//    //        }

//    //        if (string.IsNullOrWhiteSpace(request.Email))
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//    //            await context.Response.WriteAsJsonAsync("Email is required.");

//    //            return;
//    //        }

//    //        if (string.IsNullOrWhiteSpace(request.UserName))
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//    //            await context.Response.WriteAsJsonAsync("UserName is required.");

//    //            return;
//    //        }

//    //        if (string.IsNullOrWhiteSpace(request.Password))
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//    //            await context.Response.WriteAsJsonAsync("Password is required.");

//    //            return;
//    //        }

//    //        (bool IsSuccess, string Message) result;

//    //        result = await _authManager.RegisterAsync(request);


//    //        if (result.IsSuccess)
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.OK;
//    //            await context.Response.WriteAsJsonAsync("Successfully registered!");
//    //        }
//    //        else
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//    //            await context.Response.WriteAsJsonAsync("Failed to register the account.");

//    //        }
//    //    });
//    //}

//    ///// <summary>
//    ///// Maps the POST endpoint for signing in.
//    ///// </summary>
//    ///// <param name="endpoints"></param>
//    //private void MapPostSignIn(IEndpointRouteBuilder endpoints)
//    //{
//    //    endpoints.MapPost("/v1/auth/signin", async context =>
//    //    {
//    //        var request = await context.Request.ReadFromJsonAsync<SignInRequest>();

//    //        if (request is null)
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//    //            return;
//    //        }

//    //        var result = await _authManager.SignInAsync(request);

//    //        string message;

//    //        if (result.IsSuccess)
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.OK;
//    //            message = "Successfully logged in!";
//    //        }
//    //        else
//    //        {
//    //            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//    //            message = "Failed to log in";
//    //        }

//    //        await context.Response.WriteAsJsonAsync(message);
//    //    });
//    //}

//    ///// <summary>
//    ///// Maps the POST endpoint for signing out.
//    ///// </summary>
//    ///// <param name="endpoints"></param>
//    //private void MapPostSignout(IEndpointRouteBuilder endpoints)
//    //{
//    //    endpoints.MapPost("/v1/auth/signout", async context =>
//    //    {
//    //        await _authManager.SignOutAsync();

//    //        await context.Response.WriteAsJsonAsync("Successfully signed out!");
//    //    });
//    //}

//    ///// <summary>
//    ///// Maps the POST endpoint for resetting a known password.
//    ///// </summary>
//    ///// <param name="endpoints"></param>
//    //private void MapPostPasswordReset(IEndpointRouteBuilder endpoints)
//    //{
//    //    endpoints.MapPost("/v1/auth/password-reset", async context =>
//    //    {

//    //    });
//    //}

//    ///// <summary>
//    ///// Maps the POST endpoint for requesting a reset for a forgotten password.
//    ///// </summary>
//    ///// <param name="endpoints"></param>
//    //private void MapPostPasswordForgot(IEndpointRouteBuilder endpoints)
//    //{
//    //    endpoints.MapPost("/v1/auth/password-forgot", async context =>
//    //    {

//    //    });
//    //}
//}