using Deliscio.Admin.Models;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Admin.Controllers;
public class LinksController : Controller
{
    private readonly IMediator _mediator;

    private const int PAGE_SIZE = 50;


    public LinksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string term = "", [FromQuery] string tags = "", [FromQuery] string domain = "", [FromQuery] int page = 1, [FromQuery] int size = PAGE_SIZE)
    {
        var rslts = await SearchLinks(term: term, tags: tags, domain: domain, page: page, size: size);

        return View(rslts);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var queryGet = new GetLinkByIdQuery(id);
        var link = await _mediator.Send(queryGet);

        if (link is null)
            return NotFound();

        var returnUrl = HttpContext.Request.Headers["Referer"].ToString();
        var model = LinkEditDetailsModel.Success(link, returnUrl, null);

        var queryRelated = new GetLinkRelatedLinksQuery(new Guid(link.Id));
        var relateds = await _mediator.Send(queryRelated);

        model.RelatedLinks = relateds;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/links/edit")]
    public async Task<IActionResult> Edit([FromForm] LinkItem link)
    {
        if (link is null)
            return BadRequest();

        //var cmd = new UpdateLinkCommand(model.Link);
        //var rslt = await _mediator.Send(cmd);

        //if (rslt.IsSuccess)
        //    return Redirect(model.ReturnUrl);

        //model = LinkEditDetailsModel.Failure(rslt.ErrorMessage);
        //return View(model);

        return Ok();
    }

    private async Task<PagedResults<LinkItem>> SearchLinks(string term = "", string tags = "", string domain = "", int? page = 1, int? size = PAGE_SIZE)
    {
        var newPageNo = Math.Max(1, page ?? 1);
        var newPageSize = size.GetValueOrDefault() <= 10 ? PAGE_SIZE : size.GetValueOrDefault();

        var query = new FindLinksQuery(term, newPageNo, newPageSize);
        var rslts = await _mediator.Send(query);

        return rslts;
    }
}
