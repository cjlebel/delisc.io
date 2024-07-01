using Deliscio.Modules.Authentication.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Admin.Controllers;
public class RolesController : ControllerBase
{
    private readonly ILogger<RolesController> _logger;
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator, ILogger<RolesController> logger) : base(mediator)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        await WithAvailableRolesAsync();

        return View();
    }
}
