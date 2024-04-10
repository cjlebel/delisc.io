using Deliscio.Modules.Authentication.Common.Models.Requests;
using Deliscio.Modules.Authentication.MediatR.Commands;
using Deliscio.Modules.Authentication.MediatR.Requests;
using Deliscio.Modules.UserProfiles.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Admin.Controllers;
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
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

    public async Task<IActionResult> Create()
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

        var x = HttpContext.Request;

        var request = new CreateUserRequest(email, password, username, roles);

        var command = new CreateUserCommand(request);
        var response = await _mediator.Send(command);

        if (response.IsFailed)
            return TypedResults.BadRequest(response.Errors?.Select(e => e.Message).ToArray() ?? new[] { "Could not create the user" });

        return TypedResults.Ok();
    }

    public async Task<IActionResult> Profile(string id)
    {
        var query = new GetUserProfileQuery(id);

        var rslts = await _mediator.Send(query);

        if (rslts.IsFailed)
            return BadRequest(rslts.Errors);

        return View(rslts.Value);
    }


}
