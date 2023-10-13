using System.Net;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

using SubmitLinkRequest = Deliscio.Apis.WebApi.Common.Requests.SubmitLinkRequest;

namespace Deliscio.Apis.WebApi.Common.APIs;

public class LinksApiEndpoints : BaseApiEndpoints
{
    private readonly ILogger<LinksApiEndpoints> _logger;
    private readonly ILinksManager _manager;

    private const string ID_CANNOT_BE_NULL_OR_WHITESPACE = "Id cannot be null or whitespace";
    private const string LINK_COULD_NOT_BE_FOUND = "The Link for Id {0} could not be found";
    private const string LINKS_COULD_NOT_BE_FOUND = "The Links for Page {0} could not be found";
    private const string PAGE_NO_CANNOT_BE_LESS_THAN_ONE = "PageNo cannot be less than 1";
    private const string PAGE_SIZE_CANNOT_BE_LESS_THAN_ONE = "PageSize cannot be less than 1";
    private const string TAGS_CANNOT_BE_NULL_OR_EMPTY = "Tags cannot be null or empty";
    private const string TAGS_COUNT_CANNOT_BE_LESS_THAN_ONE = "TagsCount cannot be less than 1";

    private const int DEFAULT_PAGE_NO = 1;
    private const int DEFAULT_PAGE_SIZE = 25;
    private const int DEFAULT_TAG_COUNT = 25;

    public LinksApiEndpoints(ILinksManager manager, ILogger<LinksApiEndpoints> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        MapGetLink(endpoints);
        MapLinksSearch(endpoints);
        MapLinksRelated(endpoints);
        MapGetTags(endpoints);
        MapSubmitLink(endpoints);
    }

    /// <summary>
    /// Maps the endpoints that gets a single Link by its Id.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <remarks>/v1/link/fa431c01-992b-4773-a504-05b9b672a3b2</remarks>
    private void MapGetLink(IEndpointRouteBuilder endpoints)
    {
        // Id is required, so this will never be hit if id is empty (it will go to the next endpoint that has an optional pageNo and pageSize)
        endpoints.MapGet("v1/link/{linkId}",
                async ([FromRoute] string? linkId, CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrWhiteSpace(linkId))
                        return Results.BadRequest(ID_CANNOT_BE_NULL_OR_WHITESPACE);

                    var guidId = Guid.Parse(linkId);
                    if (guidId == Guid.Empty)
                        return Results.BadRequest(ID_CANNOT_BE_NULL_OR_WHITESPACE);

                    var result = await _manager.GetLinkAsync(linkId, cancellationToken);

                    return Results.Ok(result);
                })
            .Produces<Link>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("GetLink")
            .WithSummary("Gets single link item")
            .WithDescription("This endpoint retrieves a single Link item based on the id provided. If none if found, then a NotFound is returned")
            .WithName("GetLink")
            //https://stackoverflow.com/questions/70800034/add-swagger-description-to-minimal-net-6-apis
            .WithMetadata("Meta data for /links/{id}");
    }

    /// <summary>
    /// Maps the endpoints that gets a collection of Links as a page of results by the parameters that were populated.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void MapLinksSearch(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("v1/links", async ([FromQuery] string? search, [FromQuery] string? tags, [FromQuery] int? page, [FromQuery] int? count, CancellationToken cancellationToken) =>
        {
            var newPage = page ?? DEFAULT_PAGE_NO;

            if (newPage < 1)
                return Results.BadRequest(PAGE_NO_CANNOT_BE_LESS_THAN_ONE);

            var newPageSize = count ?? DEFAULT_PAGE_SIZE;

            if (newPageSize < 1)
                return Results.BadRequest(PAGE_SIZE_CANNOT_BE_LESS_THAN_ONE);

            var newSearch = search ?? string.Empty;
            var newTags = tags ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(newTags))
            {
                var tagsList = WebUtility.UrlDecode(newTags).Split(",").ToArray();

                var resultsByTags = await _manager.GetLinksByTagsAsync(tagsList, newPage, newPageSize, cancellationToken);

                return Results.Ok(resultsByTags);
            }

            if (!string.IsNullOrWhiteSpace(newSearch))
            {
                throw new NotImplementedException();
            }

            var results = await _manager.GetLinksAsync(newPage, newPageSize, cancellationToken);
            return Results.Ok(results);

            //var results = await _manager.SearchForLinksAsync(search, cancellationToken);

            //return Results.Ok(results);

            //return Results.Ok();
        })
        .Produces<PagedResults<LinkItem>>()
        .ProducesProblem((int)HttpStatusCode.OK)
        .ProducesProblem((int)HttpStatusCode.BadRequest)
        .WithDisplayName("SearchForLinks")
        .WithSummary("Search for links")
        .WithDescription("This endpoint searches for links based on the search term provided. If the search term is null or whitespace, a BadRequest is returned");
    }

    private void MapLinksRelated(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("v1/links/{linkId}/related",
                async ([FromRoute] string linkId, CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrWhiteSpace(linkId))
                        return Results.BadRequest(ID_CANNOT_BE_NULL_OR_WHITESPACE);

                    var guidId = Guid.Parse(linkId);
                    if (guidId == Guid.Empty)
                        return Results.BadRequest(ID_CANNOT_BE_NULL_OR_WHITESPACE);

                    var result = await _manager.GetRelatedLinksAsync(linkId, cancellationToken);

                    return Results.Ok(result);
                })
            .Produces<LinkItem[]>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("GetRelatedLinks")
            .WithSummary("Gets links that are related to the link with the id that was provided")
            .WithDescription("This endpoint retrieves a collection of related links based on the id provided.");
    }

    private void MapGetTags(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("v1/links/tags",
                async ([FromQuery] string? tags, [FromQuery] int? count, CancellationToken cancellationToken) =>
                {
                    var newCount = count ?? DEFAULT_TAG_COUNT;

                    if (newCount < 1)
                        return Results.BadRequest(TAGS_COUNT_CANNOT_BE_LESS_THAN_ONE);

                    var tagsArr = tags?.Split(",").ToArray() ?? Array.Empty<string>();

                    var results = await _manager.GetTagsAsync(tagsArr, newCount, cancellationToken);

                    return Results.Ok(results);
                })
            .Produces<LinkTag[]>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("GetTopTags")
            .WithSummary("Get a collection of the top tags")
            .WithDescription("");
    }

    /// <summary>
    /// Maps the endpoints that submits a new Link.
    /// </summary>
    /// <param name="endpoints"></param>
    private void MapSubmitLink(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("v1/links/",
            async (SubmitLinkRequest? request) =>
            {
                if (request is null)
                    return Results.BadRequest("Request cannot be null");

                // Temporarily hard coding this until we get authentication working
                request.SubmittedById = "48263056-61ad-b4a3-05e0-712025051842";

                var isValid = request.IsValid();

                if (!isValid.Value)
                    return Results.BadRequest($"Submit Link Failed:{Environment.NewLine}{string.Join(Environment.NewLine, isValid.Errors.Select(e => e.ErrorMessage).ToArray())}");

                var result = await _manager.SubmitLinkAsync(request.Url, request.SubmittedById, request.Title, tags: request.Tags);

                return Results.Ok(result);
            })
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("SubmitLink");
    }
}