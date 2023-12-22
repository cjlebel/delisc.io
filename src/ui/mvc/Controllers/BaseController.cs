using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Mvc.Controllers;
public class BaseController<T> : Controller
{
    protected IMediator? MediatR;

    protected ILogger<T> Logger { get; }

    protected BaseController(ILogger<T> logger)
    {
        Logger = logger;
    }

    protected BaseController(IMediator mediator, ILogger<T> logger)
    {
        Logger = logger;
        MediatR = mediator;
    }

    /// <summary>
    /// Adds a canonical URL to the ViewBag.CanonicalUrl;
    /// </summary>
    /// <param name="url">The url to be used as the canonical</param>
    /// <param name="alternateUrls">Alternate URLs to be used as alternates (optional)</param>
    // StringSyntax
    protected void WithCanonicalUrl(string url, string[]? alternateUrls = default)
    {
        ViewBag.CanonicalUrl = new Uri(url).AbsolutePath;

        if (alternateUrls != null)
        {
            ViewBag.AlternateUrls = alternateUrls.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Uri(x).AbsolutePath).ToArray();
        }
    }
}
