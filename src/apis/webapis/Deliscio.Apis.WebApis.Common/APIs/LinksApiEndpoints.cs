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
        MapGetLinksAsPager(endpoints);
        //MapGetLinksAsPagerWithPageNoPageSize(endpoints);
        MapGetLinksByTagsAsPager(endpoints);
        MapGetRelatedTagsAsPager(endpoints);
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
                        return Results.NotFound(ID_CANNOT_BE_NULL_OR_WHITESPACE);

                    var guidId = Guid.Parse(linkId);
                    if (guidId == Guid.Empty)
                        return Results.NotFound(ID_CANNOT_BE_NULL_OR_WHITESPACE);

                    var result = await _manager.GetLinkAsync(linkId, cancellationToken);

                    if (result is null)
                        return Results.NotFound(string.Format(LINK_COULD_NOT_BE_FOUND, linkId));

                    return Results.Ok(result);
                })
            .Produces<Link>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("GetLink")
            .WithSummary("Gets single link item")
            .WithDescription("This endpoint retrieves a single Link item based on the id provided. If none if found, then a NotFound is returned")
            .WithName("GetLink")
            //https://stackoverflow.com/questions/70800034/add-swagger-description-to-minimal-net-6-apis
            .WithMetadata("Meta data for /links/{id}");
    }

    //private void MapGetLinksAsPager(IEndpointRouteBuilder endpoints)
    //{
    //    // Id is required, so this will never be hit if id is empty (it will go to the next endpoint that has an optional pageNo and pageSize)
    //    endpoints.MapGet("v1/links",
    //            async (CancellationToken cancellationToken) =>
    //            {
    //                var results = await _manager.GetLinksAsync(DEFAULT_PAGE_NO, DEFAULT_PAGE_SIZE, token: cancellationToken);

    //                if (!results.Results.Any())
    //                    return Results.NotFound(string.Format(LINKS_COULD_NOT_BE_FOUND, DEFAULT_PAGE_NO));

    //                return Results.Ok(results);
    //            })
    //        .Produces<Link>()
    //        .ProducesProblem((int)HttpStatusCode.OK)
    //        .ProducesProblem((int)HttpStatusCode.NotFound)
    //        .ProducesProblem((int)HttpStatusCode.BadRequest)
    //        .WithDisplayName("GetLinksAsPage")
    //        .WithSummary("Gets a collection of links")
    //        .WithDescription("This endpoint is the default for returning a page of links")
    //        .WithName("GetLink")
    //        //https://stackoverflow.com/questions/70800034/add-swagger-description-to-minimal-net-6-apis
    //        .WithMetadata("Meta data for /links/{id}");
    //}

    /// <summary>
    /// Maps the endpoints that gets a collection of Links as a page of results.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <remarks>/v1/links/</remarks>
    /// <remarks>/v1/links/1</remarks>
    /// <remarks>/v1/links/1/25</remarks>
    private void MapGetLinksAsPager(IEndpointRouteBuilder endpoints)
    {
        // This is also the same as /v1/links where no id, pageNo, or pageSize is provided
        endpoints.MapGet("v1/links/{pageNo:int?}/{pageSize:int?}",
                async ([FromRoute] int? pageNo, [FromRoute] int? pageSize, CancellationToken cancellationToken) =>
            {
                // pageNo and pageSize are nullable, so we need to check for nulls and set default values
                var newPageNo = pageNo ?? DEFAULT_PAGE_NO;
                var newPageSize = pageSize ?? DEFAULT_PAGE_SIZE;

                if (newPageNo < 1)
                    return Results.BadRequest(PAGE_NO_CANNOT_BE_LESS_THAN_ONE);

                if (newPageSize < 1)
                    return Results.BadRequest(PAGE_SIZE_CANNOT_BE_LESS_THAN_ONE);

                var results = await _manager.GetLinksAsync(newPageNo, newPageSize, cancellationToken);

                if (!results.Results.Any())
                    return Results.NotFound(string.Format(LINKS_COULD_NOT_BE_FOUND, pageNo));

                return Results.Ok(results);
            })
            .Produces<PagedResults<Link>>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("GetLinks")
            .WithSummary("Get paginated collection of links")
            .WithDescription("This endpoint retrieves paginated links based on pageNo and pageSize. If either pageNo or pageSize is less than 1, a BadRequest is returned");
        //.WithGroupName("Links");
    }

    /// <summary>
    /// Maps the endpoints that gets a collection of Links as a page of results by the provided tags.
    /// </summary>
    /// <param name="endpoints"></param>
    private void MapGetLinksByTagsAsPager(IEndpointRouteBuilder endpoints)
    {
        // This is also the same as /v1/links where no id, pageNo, or pageSize is provided
        endpoints.MapGet("v1/links/{tags}/{pageNo:int?}/{pageSize:int?}",
                async ([FromRoute] string tags, [FromRoute] int? pageNo, [FromRoute] int? pageSize, CancellationToken cancellationToken) =>
                {
                    // pageNo and pageSize are nullable, so we need to check for nulls and set default values
                    var newPageNo = pageNo ?? DEFAULT_PAGE_NO;
                    var newPageSize = pageSize ?? DEFAULT_PAGE_SIZE;

                    if (newPageNo < 1)
                        return Results.BadRequest(PAGE_NO_CANNOT_BE_LESS_THAN_ONE);

                    if (newPageSize < 1)
                        return Results.BadRequest(PAGE_SIZE_CANNOT_BE_LESS_THAN_ONE);

                    var tagsList = tags.Split(",").ToArray();

                    var results = await _manager.GetLinksByTagsAsync(tagsList, newPageNo, newPageSize, cancellationToken);

                    if (!results.Results.Any())
                        return Results.NotFound(string.Format(LINKS_COULD_NOT_BE_FOUND, pageNo));

                    return Results.Ok(results);
                })
            .Produces<PagedResults<Link>>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("GetLinks")
            .WithSummary("Get paginated collection of links")
            .WithDescription("This endpoint retrieves paginated links based on pageNo and pageSize. If either pageNo or pageSize is less than 1, a BadRequest is returned");
    }

    /// <summary>
    /// Maps the endpoints that gets a collection of Tags that are related to the provided tags, using the Links to get the results.
    /// </summary>
    /// <param name="endpoints"></param>
    private void MapGetRelatedTagsAsPager(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("v1/links/tags/{tags}/{count:int?}",
                async ([FromRoute] string tags, [FromRoute] int? count, CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrWhiteSpace(tags))
                        return Results.BadRequest(TAGS_CANNOT_BE_NULL_OR_EMPTY);

                    var newCount = count ?? DEFAULT_TAG_COUNT;

                    if (newCount < 1)
                        return Results.BadRequest(TAGS_COUNT_CANNOT_BE_LESS_THAN_ONE);

                    var tagsList = tags.Split(",").ToArray();

                    var results = await _manager.GetRelatedTagsAsync(tagsList, newCount, cancellationToken);

                    return Results.Ok(results);
                })
            .Produces<LinkTag[]>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("GetRelatedTags")
            .WithSummary("Get a collection of tags that are related to the tags provided")
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