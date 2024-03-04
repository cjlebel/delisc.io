using Deliscio.Admin.Models;
using Deliscio.Common.Helpers;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Models.Requests;
using Deliscio.Modules.Links.MediatR.Commands;
using Deliscio.Modules.Links.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Admin.Controllers;
public class LinksController : Controller
{
    private readonly IMediator _mediator;

    private readonly Guid _deliscioUserId;

    private const int DEFAULT_PAGE_SIZE = 50;

    private const string ERROR_MISSING_LINK_ID = "The link ID is missing";
    private const string ERROR_MISSING_LINK_IDS = "The link IDs are null or empty";
    private const string ERROR_MISSING_TITLE = "The link title is missing";
    private const string ERROR_NULL_REQUEST = "The request is null";

    public LinksController(IMediator mediator)
    {
        _mediator = mediator;

        _deliscioUserId = GuidHelpers.GetMD5AsGuid("deliscio");
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? page = 1,
        [FromQuery] string? term = "", [FromQuery] string? tags = "", [FromQuery] string? domain = "",
        [FromQuery] bool? isActive = null, [FromQuery] bool? isFlagged = null, [FromQuery] bool? isDeleted = null)
    {
        /*FindLinksAdminRequest*/
        var rslts = await SearchLinks(term: term ?? string.Empty, domain: domain ?? string.Empty, page: page, size: DEFAULT_PAGE_SIZE);

        ViewBag.SelectedTags = tags ?? string.Empty;
        //ViewBag.SelectedTags = !string.IsNullOrWhiteSpace(tags?.Trim()) ? tags.Split(',') : Array.Empty<string>();

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
        var relatedLinks = await _mediator.Send(queryRelated);

        model.RelatedLinks = relatedLinks;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/links/{linkId}/edit")]
    public async Task<IActionResult> Edit([FromRoute] string linkId, [FromBody] LinkEditRequest request)
    {
        if (request! is null)
            return BadRequest(ERROR_NULL_REQUEST);

        if (string.IsNullOrWhiteSpace(request.Id))
            return BadRequest(ERROR_MISSING_LINK_ID);

        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(ERROR_MISSING_TITLE);

        var cmd = new EditLinkCommand(request, _deliscioUserId);
        var rslt = await _mediator.Send(cmd);

        //if (rslt.IsSuccess)
        //    return Redirect(model.ReturnUrl);

        //model = LinkEditDetailsModel.Failure(rslt.ErrorMessage);
        //return View(model);

        return Ok(new { rslt.IsSuccess, rslt.Message });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [Route("/links/{linkId}/delete")]
    public async Task<IActionResult> Delete([FromRoute] string linkId)
    {
        if (string.IsNullOrWhiteSpace(linkId))
            return BadRequest(ERROR_MISSING_LINK_ID);

        var cmd = new DeleteLinkCommand(new Guid(linkId), _deliscioUserId);
        var rslt = await _mediator.Send(cmd);

        return Ok(new { IsSuccess = rslt, Message = $"Link Id {linkId} was deleted" });
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [Route("/links/deletes")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deletes([FromBody] string[] linkIds)
    {
        if (linkIds.Length == 0)
            return BadRequest(ERROR_MISSING_LINK_IDS);

        var count = 0;
        var deletedLinkIds = new List<string>();

        // Should pass the collection to a command and let the repo handle it?
        // Parallel ForEach?
        foreach (var linkId in linkIds)
        {
            var cmd = new DeleteLinkCommand(new Guid(linkId), _deliscioUserId);
            if (await _mediator.Send(cmd))
            {
                deletedLinkIds.Add(linkId);
                count++;
            }
        }

        if (count == 0)
            return BadRequest(new { IsSuccess = false, LinkIds = Array.Empty<string>(), Message = "No links were deleted" });

        return Ok(new { IsSuccess = true, LinkIds = deletedLinkIds.ToArray(), Message = $"{count} of {linkIds.Length} links were deleted" });
    }

    private async Task<PagedResults<LinkItem>> SearchLinks(string term = "", string tags = "", string domain = "", bool? isActive = true, bool? isFlagged = true, bool? isDeleted = false, int? page = 1, int? size = DEFAULT_PAGE_SIZE)
    {
        var newPageNo = Math.Max(1, page ?? 1);
        var newPageSize = size.GetValueOrDefault() <= 10 ? DEFAULT_PAGE_SIZE : size.GetValueOrDefault();

        IRequest<PagedResults<LinkItem>> query;
        PagedResults<LinkItem> rslts;

        var request = new FindLinksAdminRequest(newPageNo, term, domain, tags, newPageSize, 0, isActive, isFlagged, isDeleted);

        query = new FindLinksAdminQuery(request);
        rslts = await _mediator.Send(query);

        // Because search term takes priority, can't let it fall to default
        //if (!string.IsNullOrWhiteSpace(term))
        //{
        //    var request = new FindLinksAdminRequest(newPageNo, term, string.Empty, tags, newPageSize, 0, isActive, isFlagged, isDeleted);

        //    query = new FindLinksAdminQuery(request);
        //    rslts = await _mediator.Send(query);
        //}
        //else if (!string.IsNullOrWhiteSpace(tags))
        //{
        //    query = new GetLinksByTagsQuery(newPageNo, newPageSize, tags);
        //    rslts = await _mediator.Send(query);
        //}
        //else if (!string.IsNullOrWhiteSpace(domain))
        //{
        //    query = new GetLinksByDomainQuery(domain, newPageNo, newPageSize);
        //    rslts = await _mediator.Send(query);
        //}
        //else
        //{
        //    // When all else fails, ...
        //    var request = new FindLinksAdminRequest(newPageNo, string.Empty, string.Empty, tags, newPageSize, 0, isActive, isFlagged, isDeleted);

        //    query = new FindLinksQuery(request);
        //    rslts = await _mediator.Send(query);
        //}


        return rslts;
    }
}
