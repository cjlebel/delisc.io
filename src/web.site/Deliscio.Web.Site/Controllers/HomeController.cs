using System.Diagnostics;
using Ardalis.GuardClauses;
using Deliscio.Web.Site.Managers;
using Deliscio.Web.Site.ViewModels;
using Microsoft.AspNetCore.Mvc;
using RedisCaching;
using Structurizr.Annotations;

namespace Deliscio.Web.Site.Controllers;

[Component(Description = "The Deliscio Website - Home", Technology = "C#")]
[UsedByPerson("End Users", Description = "Public Webpage")]
public class HomeController : BaseController<HomeController>
{
    private readonly IHomePageManager _pageManager;

    //public HomeController(IHomePageManager pageManager, ILogger<HomeController> logger) : base(logger)
    //{
    //    Guard.Against.Null(pageManager);

    //    _pageManager = pageManager;
    //}

    public HomeController(IHomePageManager pageManager, ILogger<HomeController> logger, IRedisCaching? caching = default) : base(logger, caching)
    {
        Guard.Against.Null(pageManager);

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

    [Route("tags")]
    public async Task<IActionResult> Tags(string? tags = default, CancellationToken token = default)
    {
        try
        {
            var model = await _pageManager.GetHomePageViewModelAsync(token);

            var breadCrumbs = new Dictionary<string, string>
            {
                { "Home", "/" },
                { "Tags", "" }
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
