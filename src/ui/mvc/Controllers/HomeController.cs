using System.Diagnostics;
using Ardalis.GuardClauses;
using Deliscio.Web.Mvc.Managers;
using Deliscio.Web.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Structurizr.Annotations;

namespace Deliscio.Web.Mvc.Controllers;

[Component(Description = "The Deliscio Website - Home", Technology = "C#")]
[UsedByPerson("End Users", Description = "Public Webpage")]
public class HomeController : BaseController<HomeController>
{
    private readonly IHomePageManager _pageManager;

    public HomeController(IHomePageManager pageManager, ILogger<HomeController> logger) : base(logger)
    {
        Guard.Against.Null(pageManager);
        Guard.Against.Null(logger);

        _pageManager = pageManager;
    }

    public async Task<IActionResult> Index(CancellationToken token = default)
    {
        try
        {
            var model = await _pageManager.GetHomePageViewModelAsync(token);

            if (model is null)
                return NotFound();

            var breadCrumbs = new Dictionary<string, string>
            {
                { "Home", "/" }
            };

            ViewBag.BreadCrumbs = breadCrumbs;

            return View(model);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { PageTitle = "Error", RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
