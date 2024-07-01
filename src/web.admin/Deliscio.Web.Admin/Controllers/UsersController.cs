using Deliscio.Modules.Authentication.Common.Models;
using Deliscio.Modules.Authentication.Common.Models.Requests;
using Deliscio.Modules.Authentication.MediatR.Commands;
using Deliscio.Modules.Authentication.MediatR.Queries;
using Deliscio.Modules.UserProfiles.Common.Errors;
using Deliscio.Modules.UserProfiles.Common.Models;
using Deliscio.Modules.UserProfiles.Common.Models.Requests;
using Deliscio.Modules.UserProfiles.MediatR.Commands;
using Deliscio.Modules.UserProfiles.MediatR.Queries;
using Deliscio.Web.Admin.Models;
using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Admin.Controllers;
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) : base(mediator)
    {
        _mediator = mediator;
    }

    //public async Task<Results<Ok<PagedResults<UserProfileItem>>, BadRequest<IError>>> Index()
    public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 50)
    {
        //var users = _mediator.Send(new GetUsersQuery());
        var userQuery = new GetUsersQuery(pageNo, pageSize);

        var rslts = await _mediator.Send(userQuery);

        return View(rslts);
    }

    public async Task<IActionResult> New()
    {
        var rolesQuery = new GetRolesQuery();
        var roles = await _mediator.Send(rolesQuery);

        ViewBag.AvailableRoles = roles.Value ?? [];

        return View();
    }

    [HttpPost]
    //[Route("api/users/create")]
    public async Task<Results<Ok, BadRequest<string[]>>> Create([FromForm] string username, [FromForm] string email, [FromForm] string password, [FromForm] string[] roles)
    {
        if (string.IsNullOrWhiteSpace(username))
            return TypedResults.BadRequest(new[] { "Username is required" });

        if (string.IsNullOrWhiteSpace(email))
            return TypedResults.BadRequest(new[] { "Email is required" });

        if (string.IsNullOrWhiteSpace(password))
            return TypedResults.BadRequest(new[] { "Password is required" });

        var newUserRequest = new CreateAuthUserRequest(email, password, username, roles);
        var newUserCommand = new CreateAuthUserCommand(newUserRequest);
        var response = await _mediator.Send(newUserCommand);

        if (response.IsFailed || response.ValueOrDefault is null)
            return TypedResults.BadRequest(response.Errors?.Select(e => e.Message).ToArray() ?? new[] { "Could not create the user" });

        var user = response.ValueOrDefault;

        _ = await CreateNewUserProfile(user.Id, user.Username, user.Email, user.DateCreated);

        return TypedResults.Ok();
    }

    [HttpGet]
    [Route("users/profile/{userId}")]
    public async Task<IActionResult> Profile(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("User Profile Id is required");

        // Get th
        var userQuery = new GetUserQuery(userId);
        var userResult = await _mediator.Send(userQuery);

        if (userResult.IsFailed || userResult.Value is null)
            return BadRequest(userResult.Errors?.Select(e => e.Message).ToArray() ?? new[] { "Could not find the user" });

        var user = userResult.Value;

        UserProfile userProfile;
        var userProfileQuery = new GetUserProfileQuery(userId);

        var userProfileResult = await _mediator.Send(userProfileQuery);

        if (userProfileResult.IsFailed || userProfileResult.ValueOrDefault is null)
        {
            if (userProfileResult.Errors.Any(e => e is UserProfileNotFound))
            {
                userProfile = await CreateNewUserProfile(user.Id, user.Username, user.Email, user.DateCreated);
            }

            return BadRequest(userProfileResult.Errors?.Select(e => e.Message).ToArray() ?? new[] { "Could not find the user profile" });
        }

        userProfile = userProfileResult.ValueOrDefault;

        var model = new UserProfileViewModel(user, userProfile, Array.Empty<Role>());

        await WithAvailableRolesAsync();

        return View(model);
    }

    private async Task<UserProfile> CreateNewUserProfile(string userId, string displayName, string email, DateTimeOffset dateRegistered)
    {
        var request = new CreateUserProfileRequest(userId, displayName, email, dateRegistered);

        var command = new CreateUserProfileCommand(request);
        var response = await _mediator.Send(command);

        if (response.IsFailed)
            throw new Exception("Could not create the user");

        return response.Value;
    }
}
