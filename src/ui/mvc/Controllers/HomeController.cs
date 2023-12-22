 using System.Diagnostics;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Web.Mvc.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Structurizr.Annotations;

namespace Deliscio.Web.Mvc.Controllers;

[Component(Description = "The Deliscio Website", Technology = "C#")]
[UsedByPerson("End Users", Description = "Public APIs")]
public class HomeController : BaseController<HomeController>
{
    public HomeController(IMediator mediator, ILogger<HomeController> logger) : base(mediator, logger)
    {

    }

    public async Task<IActionResult> Index(int pageNo = 1, int pageSize = 50, CancellationToken token = default)
    {
        var query = new GetLinksQuery(pageNo, pageSize);

        var results = await MediatR!.Send(query, token);

        return View(results);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
