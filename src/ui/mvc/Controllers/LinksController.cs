using System.Text;
using Ardalis.GuardClauses;
using Deliscio.Web.Mvc.Managers;
using Microsoft.AspNetCore.Mvc;
using Structurizr.Annotations;

namespace Deliscio.Web.Mvc.Controllers;

[Component(Description = "The Deliscio Website - Links", Technology = "C#")]
[UsedByPerson("End Users", Description = "Public Webpage")]
public class LinksController : BaseController<LinksController>
{
    private readonly int _defaultPageSize;

    private readonly ILinksPageManager _linksManager;

    private const string PAGE_NUMBER_GREATER_THAN_ZERO = "Page number must be greater than 0.";
    private const string COUNT_GREATER_THAN_ZERO = "Count must be greater than 0.";
    private const string SKIP_GREATER_THAN_ZERO = "Skip must be greater or equal to 0.";

    public LinksController(ILinksPageManager linksManager, ILogger<LinksController> logger) : base(logger)
    {
        Guard.Against.Null(linksManager);
        Guard.Against.Null(logger);

        _defaultPageSize = linksManager.DefaultPageSize;

        _linksManager = linksManager;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p">Page Number</param>
    /// <param name="c">Count</param>
    /// <param name="s">Skip</param>
    /// <param name="t">Tags</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<IActionResult> Index([FromQuery] int? p = 1, [FromQuery] int? s = 0, [FromQuery] string? t = "", CancellationToken token = default)
    {
        var pageNo = p.GetValueOrDefault(1);
        var skip = s.GetValueOrDefault(0);
        var tags = t?.Trim();

        if (pageNo < 1)
            return BadRequest(PAGE_NUMBER_GREATER_THAN_ZERO);

        if (skip < 0)
            return BadRequest(SKIP_GREATER_THAN_ZERO);

        try
        {
            var model = await _linksManager.GetLinksPageViewModelAsync(pageNo, skip, tags, token);

            if (model is null)
                return NotFound();

            var breadCrumbs = new Dictionary<string, string>
            {
                { "Home", "/" },
                { "Links", "/links" }
            };

            if (model.Tags is { Length: > 0 })
            {
                var queryString = new StringBuilder();

                for (var i = 0; i < model.Tags.Length; i++)
                {
                    var tag = model.Tags[i];

                    if (i > 0)
                        queryString.Append(',');

                    queryString.Append(tag);

                    breadCrumbs.Add(tag, $"/links?t={queryString}");
                }
            }

            ViewBag.BreadCrumbs = breadCrumbs;

            return View(model);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Route("links/{id:guid}")]
    public async Task<IActionResult> Details([FromRoute] Guid id, CancellationToken token = default)
    {
        if (id == Guid.Empty)
            return NotFound();

        try
        {
            var model = await _linksManager.GetLinksDetailsPageViewModelAsync(id, token);

            if (model is null)
                return NotFound();

            var breadCrumbs = new Dictionary<string, string>
            {
                { "Home", "/" },
                { "Links", "/links" },
                { model.Title, $"/links/{model.Id}" }
            };

            ViewBag.BreadCrumbs = breadCrumbs;

            return View(model);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
