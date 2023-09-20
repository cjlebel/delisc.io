using System.Net;
using Deliscio.Apis.WebApi.Common.Responses;
using Deliscio.Modules.Links.Interfaces;
using Deliscio.Modules.Links.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Deliscio.Apis.WebApi.Common.APIs;

public class LinksApiEndpoints : BaseApiEndpoints
{
    private readonly ILinksService _linksService;
    private readonly ILogger<LinksApiEndpoints> _logger;

    private const string ID_CANNOT_BE_NULL_OR_WHITESPACE = "Id cannot be null or whitespace";
    private const string LINK_COULD_NOT_BE_FOUND = "The Link for Id {0} could not be found";
    private const string LINKS_COULD_NOT_BE_FOUND = "The Links for Page {0} could not be found";
    private const string PAGENO_CANNOT_BE_LESS_THAN_ONE = "PageNo cannot be less than 1";
    private const string PAGESIZE_CANNOT_BE_LESS_THAN_ONE = "PageSize cannot be less than 1";

    public LinksApiEndpoints(ILinksService linksService, ILogger<LinksApiEndpoints> logger)
    {
        _linksService = linksService;
        _logger = logger;
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        MapGetLink(endpoints);
        MapGetLinksAsPager(endpoints);

    }

    private void MapGetLink(IEndpointRouteBuilder endpoints)
    {
        // Id is required, so this will never be hit if id is empty (it will go to the next endpoint that has an optional pageNo and pageSize)
        endpoints.MapGet("v1/links/{id}",
                async ([FromRoute] string id, CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrWhiteSpace(id))
                        return Results.BadRequest(ID_CANNOT_BE_NULL_OR_WHITESPACE);

                    var result = await _linksService.GetAsync(id, cancellationToken);

                    if (result is null)
                        return Results.NotFound(string.Format(LINK_COULD_NOT_BE_FOUND, id));

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

    private void MapGetLinksAsPager(IEndpointRouteBuilder endpoints)
    {
        // This is also the same as /v1/links where no id, pageNo, or pageSize is provided
        endpoints.MapGet("v1/links/{pageNo:int?}/{pageSize:int?}",
                async ([FromRoute] int? pageNo, [FromRoute] int? pageSize, CancellationToken cancellationToken) =>
            {
                // pageNo and pageSize are nullable, so we need to check for nulls and set default values
                var newPageNo = pageNo ?? 1;
                var newPageSize = pageSize ?? 25;

                if (newPageNo < 1)
                    return Results.BadRequest(PAGENO_CANNOT_BE_LESS_THAN_ONE);

                if (newPageSize < 1)
                    return Results.BadRequest(PAGESIZE_CANNOT_BE_LESS_THAN_ONE);

                var results = await _linksService.GetAsync(newPageNo, newPageSize, cancellationToken);

                if (!results.Results.Any())
                    return Results.NotFound(string.Format(LINKS_COULD_NOT_BE_FOUND, pageNo));

                var pager = GetPager(results.Results, newPageNo, newPageSize, results.TotalCount);

                return Results.Ok(pager);
            })
            .Produces<PagedResponse<Link>>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .WithDisplayName("GetLinks")
            .WithSummary("Get paginated collection of links")
            .WithDescription("This endpoint retrieves paginated links based on pageNo and pageSize. If either pageNo or pageSize is less than 1, a BadRequest is returned");
        //.WithGroupName("Links");
    }
}