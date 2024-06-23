using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Admin.Controllers;

[Authorize(Roles = "Admin")]
public abstract class ControllerBase : Controller
{
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
}
