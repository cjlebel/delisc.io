using Deliscio.Admin.Models;
using Deliscio.Admin.Models.Responses;
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
    private const string ERROR_PAGE_NO_LESS_THAN_ONE = "The page number must be greater than 0";
    private const string ERROR_PAGE_SIZE_LESS_THAN_ONE = "The page size must be greater than 0";

    public LinksController(IMediator mediator)
    {
        _mediator = mediator;

        _deliscioUserId = GuidHelpers.GetMD5AsGuid("deliscio");
    }

    //[HttpGet]
    ////public async Task<IActionResult> Index([FromQuery] int? page = 1,
    ////    [FromQuery] string? term = "", [FromQuery] string? tags = "", [FromQuery] string? domain = "",
    ////    [FromQuery] bool? isActive = null, [FromQuery] bool? isFlagged = null, [FromQuery] bool? isDeleted = null)
    //public async Task<IActionResult> Index([FromBody] LinksSearchRequest? request = null)
    //{
    //    if (request is null)
    //        request = new LinksSearchRequest();

    //    if (request.PageNo <= 0)
    //        return BadRequest(ERROR_PAGE_NO_LESS_THAN_ONE);

    //    if (request.PageSize <= 0)
    //        return BadRequest(ERROR_PAGE_SIZE_LESS_THAN_ONE);

    //    var rslts = await SearchLinks(term: request.SearchTerm, domain: request.Domain, tags: request.Tags, pageNo: request.PageNo, pageSize: request.PageSize, isActive: request.IsActive, isFlagged: request.IsFlagged, isDeleted: request.IsDeleted);

    //    //ViewBag.SelectedTags = tags ?? string.Empty;
    //    //ViewBag.SelectedTags = !string.IsNullOrWhiteSpace(tags?.Trim()) ? tags.Split(',') : Array.Empty<string>();

    //    var searchResults = LinksSearchResponse<LinkItem>.Create(rslts, request.SearchTerm, request.Domain, request.Tags, request.IsActive, request.IsFlagged, request.IsDeleted);

    //    return View(searchResults);
    //}

    //[HttpGet]
    //public async Task<IActionResult> Index()
    //{
    //    var rslts = await SearchLinks(isActive: true, isFlagged: false, isDeleted: false, pageNo: 1, pageSize: DEFAULT_PAGE_SIZE);

    //    var searchResults = LinksSearchResponse<LinkItem>.Create(rslts, string.Empty, string.Empty, string.Empty, true, false, false);

    //    return View(searchResults);
    //}

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? page = 1,
        [FromQuery] string? term = "", [FromQuery] string? tags = "", [FromQuery] string? domain = "",
        [FromQuery] bool? isActive = null, [FromQuery] bool? isFlagged = null, [FromQuery] bool? isDeleted = null)
    {
        if (page <= 0)
            return BadRequest(ERROR_PAGE_NO_LESS_THAN_ONE);

        var rslts = await SearchLinks(term: term ?? string.Empty, domain: domain ?? string.Empty, tags ?? string.Empty, isActive: isActive ?? true, isFlagged: isFlagged ?? false, isDeleted: isDeleted ?? false, pageNo: page, pageSize: DEFAULT_PAGE_SIZE);

        // Add tags that were searched for to the ViewBag, so that we can highlight the tags in the search results
        ViewBag.SelectedTags = tags ?? string.Empty;
        //ViewBag.SelectedTags = !string.IsNullOrWhiteSpace(tags?.Trim()) ? tags.Split(',') : Array.Empty<string>();

        var searchResults = LinksSearchResponse<LinkItem>.Create(rslts, term ?? string.Empty, domain ?? string.Empty, tags ?? string.Empty, isActive ?? true, isFlagged ?? false, isDeleted ?? false);

        return View(searchResults);
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

    private async Task<PagedResults<LinkItem>> SearchLinks(string term = "", string domain = "", string tags = "",
        bool? isActive = true, bool? isFlagged = true, bool? isDeleted = false,
        int? pageNo = 1, int? pageSize = DEFAULT_PAGE_SIZE)
    {
        var newPageNo = Math.Max(1, pageNo ?? 1);
        var newPageSize = pageSize.GetValueOrDefault() <= 10 ? DEFAULT_PAGE_SIZE : pageSize.GetValueOrDefault();

        IRequest<PagedResults<LinkItem>> query;
        PagedResults<LinkItem> rslts;

        var request = new FindLinksAdminRequest(newPageNo, term, domain, tags, newPageSize, 0, isActive, isFlagged, isDeleted);

        query = new FindLinksAdminQuery(request);
        rslts = await _mediator.Send(query);

        return rslts;
    }
}
