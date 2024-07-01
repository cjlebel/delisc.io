using Deliscio.Modules.Links.Application.Commands.DeleteLink;
using Deliscio.Modules.Links.Application.Commands.DeleteLinks;
using Deliscio.Modules.Links.Application.Commands.EditLink;
using Deliscio.Modules.Links.Application.Commands.SetLinkActivateState;
using Deliscio.Modules.Links.Application.Commands.UnDeleteLink;
using Deliscio.Modules.Links.Application.Contracts;
using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Application.Queries.GetLinkById;
using Deliscio.Modules.Links.Application.Queries.GetRelatedLinks;
using Deliscio.Modules.Links.Application.Queries.SearchLinks;
using Deliscio.Web.Admin.Models;
using Deliscio.Web.Admin.Models.Requests;
using Deliscio.Web.Admin.Models.Responses;
using FluentResults;

using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Admin.Controllers;
public class LinksController : ControllerBase
{
    private readonly ILinksModule _linksModule;
    private readonly ILogger<LinksController> _logger;

    private const int DEFAULT_PAGE_SIZE = 50;

    private const string ERROR_NULL_REQUEST = "The request is null";

    private const string ERROR_MISSING_LINK_ID = "The link ID is missing";
    private const string ERROR_MISSING_LINK_IDS = "The link IDs are null or empty";
    private const string ERROR_MISSING_TITLE = "The link title is missing";

    private const string ERROR_PAGE_SEARCH_FAILED = "Failed to search for links";
    private const string ERROR_PAGE_NO_LESS_THAN_ONE = "The page number must be greater than 0";
    private const string ERROR_PAGE_SIZE_LESS_THAN_ONE = "The page's size must be greater than 0";
    private const string ERROR_PAGE_OFFSET_LESS_THAN_ZERO = "The page's offset must be greater or equal to 0";

    public LinksController(ILinksModule linksModule, ILogger<LinksController> logger)
    {
        _linksModule = linksModule;
        _logger = logger;
    }


    [HttpGet]
    //public async Task<IActionResult> Index([FromQuery] int? page = 1, [FromQuery] int? pageSize = DEFAULT_PAGE_SIZE, [FromQuery] int? skip = 0,
    //    [FromQuery] string? term = "", [FromQuery] string? tags = "", [FromQuery] string? domain = "",
    //    [FromQuery] bool? isActive = false, [FromQuery] bool? isFlagged = false, [FromQuery] bool? isDeleted = false, CancellationToken token = default)
    public async Task<IActionResult> Index([FromQuery] LinksSearchQueryRequest request, CancellationToken token = default)
    {
        var results = await SearchLinks(term: request.Term ?? string.Empty,
            domain: request.Domain ?? string.Empty,
            tags: request.Tags ?? string.Empty,
            isActive: request.IsActive ?? true,
            isFlagged: request.IsFlagged ?? false,
            isDeleted: request.IsDeleted ?? false,
            pageNo: request.Page,
            pageSize: request.Size,

            token: token);

        if (results.IsFailed)
            return BadRequest(results.Errors?[0]?.Message ?? ERROR_PAGE_SEARCH_FAILED);

        // Add tags that were searched for to the ViewBag, so that we can highlight the tags in the search results
        ViewBag.SelectedTags = request.Tags ?? string.Empty;
        ViewBag.SelectedTags2 = !string.IsNullOrWhiteSpace(request.Tags?.Trim()) ? request.Tags.Split(',') : Array.Empty<string>();

        return View(results.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest(ERROR_MISSING_LINK_ID);

        var queryLink = new GetLinkByIdQuery(id);
        var result = await _linksModule.ExecuteAsync(queryLink, token);

        if (result.IsFailed)
            return NotFound(result.Errors);

        var link = result.Value;

        var queryRelated = new GetRelatedLinksQuery(id, 25, true);
        var relatedLinksResults = await _linksModule.ExecuteAsync(queryRelated, token);
        var relatedLinks = Array.Empty<RelatedLinkDto>();

        if (relatedLinksResults.IsSuccess)
            relatedLinks = relatedLinksResults?.Value ?? Array.Empty<RelatedLinkDto>();

        var returnUrl = HttpContext.Request.Headers["Referer"].ToString();

        var model = LinkEditDetailsModel.Ok(link, returnUrl, relatedLinks);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/links/{linkId}/edit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Edit([FromRoute] string linkId, [FromBody] EditLinkRequest request, CancellationToken token = default)
    {
        // Yes, it is possible to receive a null request, even though LinkEditRequest request is not nullable.
        if (request is null)
            return BadRequest(ERROR_NULL_REQUEST);

        if (string.IsNullOrWhiteSpace(request.LinkId))
            return BadRequest(ERROR_MISSING_LINK_ID);

        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(ERROR_MISSING_TITLE);

        var cmd = new EditLinkCommand(request, CurrentUserId);
        var rslt = await _linksModule.ExecuteAsync(cmd, token);

        if (rslt.IsFailed)
            return BadRequest(rslt.Errors);

        return Ok(new JsonResult(new ApiResponse
        {
            IsSuccess = true,
            Message = $"The link '${linkId}' was updated"
        }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/links/{linkId}/activate/{isActive:bool}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate([FromRoute] string linkId, [FromRoute] bool isActive, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(linkId))
            return BadRequest(ERROR_MISSING_LINK_ID);

        var cmd = new SetLinkActiveStateCommand(linkId, isActive, CurrentUserId);
        var rslt = await _linksModule.ExecuteAsync(cmd, token);

        if (rslt.IsFailed)
            return BadRequest("Could not update the active state for the link");

        return Ok(new JsonResult(new ApiResponse
        {
            IsSuccess = true, 
            Message = $"The link '${linkId}' was {(isActive ? "activated" : "deactivated")}"
        }));
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [Route("/links/{linkId}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromRoute] string linkId, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(linkId))
            return BadRequest(ERROR_MISSING_LINK_ID);

        var cmd = new DeleteLinkCommand(linkId, CurrentUserId);
        var rslt = await _linksModule.ExecuteAsync(cmd, token);

        if (rslt.IsFailed)
            return BadRequest("Could not delete the link");

        return Ok(new JsonResult(new ApiResponse
        {
            IsSuccess = true, 
            Message = $"The link '${linkId}' was deleted"
        }));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/links/{linkId}/undelete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UnDelete([FromRoute] string linkId, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(linkId))
            return BadRequest(ERROR_MISSING_LINK_ID);

        var cmd = new UnDeleteLinkCommand(linkId, CurrentUserId);
        var rslt = await _linksModule.ExecuteAsync(cmd, token);

        if (rslt.IsFailed)
            return BadRequest("Could not delete the link");

        return Ok(new JsonResult(new ApiResponse
        {
            IsSuccess = true, 
            Message = $"The link '${linkId}' was undeleted"
        }));
    }

    /// <summary>
    /// Deletes one or more links
    /// </summary>
    /// <param name="linkIds">Collection of link ids to be deleted</param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpDelete]
    [ValidateAntiForgeryToken]
    [Route("/links/deletes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deletes([FromBody] List<string> linkIds, CancellationToken token = default)
    {
        var linksIdsArray = linkIds?.ToArray() ?? Array.Empty<string>();

        if (linkIds is null || linksIdsArray.Length == 0)
            return BadRequest(ERROR_MISSING_LINK_IDS);

        var cmd = new DeleteLinksCommand(linkIds.ToArray(), CurrentUserId);
        var rslt = await _linksModule.ExecuteAsync(cmd, token);

        if (rslt.IsFailed)
            return BadRequest("Could not delete the links");

        var response = rslt.Value;

        return Ok(new ApiDataResponse<string[]>
        {
            IsSuccess = true,
            Data = response.ToArray(),
            Message = $"{response.Length} of {linksIdsArray.Length} links were deleted"
        });
    }

    private async Task<Result<SearchLinksQueryResponse>> SearchLinks(string term = "", string domain = "", string tags = "",
        bool? isActive = true, bool? isFlagged = true, bool? isDeleted = false,
        int? pageNo = 1, int? pageSize = DEFAULT_PAGE_SIZE, int? offset = 0,
        CancellationToken token = default)
    {
        if (pageNo <= 0)
            return Result.Fail(ERROR_PAGE_NO_LESS_THAN_ONE);

        if (pageSize <= 0)
            return Result.Fail(ERROR_PAGE_SIZE_LESS_THAN_ONE);

        if (offset < 0)
            return Result.Fail(ERROR_PAGE_OFFSET_LESS_THAN_ZERO);

        var query = SearchLinksQuery.Advanced(pageNo ?? 1, pageSize ?? DEFAULT_PAGE_SIZE, offset ?? 0, term, domain, tags, isActive, isFlagged, isDeleted);
        var results = await _linksModule.ExecuteAsync(query, token);

        if (results.IsFailed)
        {
            _logger.LogError("Failed to search for links: {Errors}", results.Errors);
            return Result.Fail(results.Errors);
        }

        return Result.Ok(results.Value);
    }
}
