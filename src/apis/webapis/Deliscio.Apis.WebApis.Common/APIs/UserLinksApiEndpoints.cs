using System.Net;
using System.Security.Cryptography;
using System.Text;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Modules.UserLinks.Common.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Deliscio.Apis.WebApi.Common.APIs;

public class UserLinksApiEndpoints : BaseApiEndpoints
{
    private readonly ILogger<LinksApiEndpoints> _logger;
    private readonly IUserLinksManager _manager;

    private const string PAGE_NO_CANNOT_BE_LESS_THAN_ONE = "PageNo cannot be less than 1";
    private const string PAGE_SIZE_CANNOT_BE_LESS_THAN_ONE = "PageSize cannot be less than 1";
    private const string TAGS_CANNOT_BE_NULL_OR_EMPTY = "Tags cannot be null or empty";
    private const string TAGS_COUNT_CANNOT_BE_LESS_THAN_ONE = "TagsCount cannot be less than 1";

    private const string USER_ID_CANNOT_BE_NULL_OR_WHITESPACE = "AuthUser's Id cannot be null or whitespace";
    private const string USER_LINK_COULD_NOT_BE_FOUND = "The AuthUser's Link for Id {0} could not be found";
    private const string USER_LINK_COULD_NOT_BE_SAVED = "Could not save the link to the user{0}AuthUser: {1}\n{0}Link: {2}";
    private const string USER_LINK_ID_CANNOT_BE_NULL_OR_WHITESPACE = "AuthUser Link's Id cannot be null or whitespace";
    private const string USER_LINKS_COULD_NOT_BE_FOUND = "The Links for Page {0} could not be found";


    private const int DEFAULT_PAGE_NO = 1;
    private const int DEFAULT_PAGE_SIZE = 25;
    private const int DEFAULT_TAG_COUNT = 25;

    public UserLinksApiEndpoints(IUserLinksManager manager, ILogger<LinksApiEndpoints> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        MapGetUsersLink(endpoints);
        MapGetUsersLinks(endpoints);

        MapPostUserAddLink(endpoints);
    }

    /// <summary>
    /// Maps the endpoints that gets a single Link by its Id.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <remarks>/v1/link/fa431c01-992b-4773-a504-05b9b672a3b2</remarks>
    private void MapGetUsersLink(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("v1/users/{userName}/link/{linkId}",
            async ([FromRoute] string userName, [FromRoute] string linkId, CancellationToken cancellationToken) =>
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return Results.BadRequest(USER_ID_CANNOT_BE_NULL_OR_WHITESPACE);

                if (string.IsNullOrWhiteSpace(linkId))
                    return Results.BadRequest(USER_LINK_ID_CANNOT_BE_NULL_OR_WHITESPACE);

                var userId = GetUserId(userName);

                var result = await _manager.GetUserLinkAsync(userId, linkId, cancellationToken);

                if (result == null)
                    return Results.NotFound(string.Format(USER_LINK_COULD_NOT_BE_FOUND, linkId));

                return Results.Ok(result);
            })
            .Produces<UserLink>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.BadRequest);
    }

    private void MapGetUsersLinks(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("v1/users/{userName}/links/{pageNo:int?}/{pageSize:int?}",
                async ([FromRoute] string userName, [FromRoute] int? pageNo, [FromRoute] int? pageSize, CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrWhiteSpace(userName))
                        return Results.BadRequest(USER_ID_CANNOT_BE_NULL_OR_WHITESPACE);

                    var newPageNo = pageNo ?? DEFAULT_PAGE_NO;
                    var newPageSize = pageSize ?? DEFAULT_PAGE_SIZE;

                    if (newPageNo < 1)
                        return Results.BadRequest(PAGE_NO_CANNOT_BE_LESS_THAN_ONE);

                    if (newPageSize < 1)
                        return Results.BadRequest(PAGE_SIZE_CANNOT_BE_LESS_THAN_ONE);

                    var userId = GetUserId(userName);

                    var results = await _manager.GetUserLinksAsync(userId, newPageNo, newPageSize, cancellationToken);

                    if (!results.Items.Any())
                        return Results.NotFound(string.Format(USER_LINKS_COULD_NOT_BE_FOUND, pageNo));

                    return Results.Ok(results);
                })
            .Produces<UserLink>()
            .ProducesProblem((int)HttpStatusCode.OK)
            .ProducesProblem((int)HttpStatusCode.NotFound)
            .ProducesProblem((int)HttpStatusCode.BadRequest);
    }

    /// <summary>
    /// Maps the endpoint that allows a user to add an existing link to their profile.
    /// </summary>
    /// <param name="endpoints"></param>
    private void MapPostUserAddLink(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("v1/users/{userName}/links/add",
            async ([FromRoute] string userName, AddUserLinkRequest request, CancellationToken cancellationToken) =>
            {
                var userId = GetUserId(userName);

                if (string.IsNullOrWhiteSpace(userId))
                    return Results.BadRequest(USER_ID_CANNOT_BE_NULL_OR_WHITESPACE);

                if (request.LinkId == Guid.Empty.ToString())
                    return Results.BadRequest(USER_LINK_ID_CANNOT_BE_NULL_OR_WHITESPACE);

                var result = await _manager.AddLinkAsync(userId, request.LinkId, request.Title, request.Tags, request.IsPrivate, cancellationToken);

                if (string.IsNullOrWhiteSpace(result) || result == Guid.Empty.ToString())
                    return Results.NotFound(string.Format(USER_LINK_COULD_NOT_BE_SAVED, Environment.NewLine, userId, request.LinkId));

                return Results.Ok(result);
            })
    .Produces<string>()
    .ProducesProblem((int)HttpStatusCode.OK)
    .ProducesProblem((int)HttpStatusCode.NotFound)
    .ProducesProblem((int)HttpStatusCode.BadRequest);
    }

    private static string GetUserId(string username)
    {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = MD5.HashData(Encoding.Default.GetBytes(username));

        return new Guid(data).ToString();
    }
}
