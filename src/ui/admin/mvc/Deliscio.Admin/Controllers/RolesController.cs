using Deliscio.Modules.Authentication.MediatR.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Admin.Controllers;
public class RolesController : Controller
{
    private readonly ILogger<RolesController> _logger;
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator, ILogger<RolesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var query = new GetRolesQuery();

        var rslts = await _mediator.Send(query);

        return View(rslts);
    }
}
