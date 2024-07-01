using System.Security.Claims;
using Deliscio.Modules.Authentication.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Admin.Controllers;

[Authorize(Roles = "Admin")]
public abstract class ControllerBase : Controller
{
    protected IMediator? Mediator { get; }

    public string CurrenUserName
    {
        get => User.FindFirstValue(ClaimTypes.Name) ?? "Guest";
    }

    public string CurrentUserId
    {
        //var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier) // will give the user's userId
        // var userName =  User.FindFirstValue(ClaimTypes.Name) 
        get => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }

    protected ControllerBase() { }

    protected ControllerBase(IMediator mediator)
    {
        Mediator = mediator;
    }

    protected async Task WithAvailableRolesAsync()
    {
        var query = new GetRolesQuery();

        var rslts = await Mediator.Send(query);

        ViewBag.AvailableRoles = rslts.Value ?? [];
    }
}
