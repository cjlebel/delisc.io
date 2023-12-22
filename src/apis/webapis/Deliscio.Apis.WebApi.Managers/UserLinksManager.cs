using Ardalis.GuardClauses;
using Deliscio.Apis.WebApi.Common.Abstracts;
using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Modules.UserLinks.Common.Models;
using Deliscio.Modules.UserLinks.MediatR.Commands;
using Deliscio.Modules.UserLinks.MediatR.Queries;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Deliscio.Apis.WebApi.Managers;

/// <summary>
/// Facilitates the management of a AuthUser's Links between the API and the UserLinks module.
/// Although in most cases this may be/seem redundant, there could be extra logic that the API needs,
/// which would be handled here instead of in the APIs themselves (biz logic does not belong in APIs or Controllers).
///
/// For example, what if the caller wanted details for a Page, such as SEO details? This would be handled here.
/// Or if other services need to be used, it too would be handled here.
/// </summary>
public sealed class UserLinksManager : ManagerBase<UserLinksManager>, IUserLinksManager
{
    private readonly IBusControl _bus;
    private readonly IMediator _mediator;

    public UserLinksManager(IMediator mediator, IBusControl bus, ILogger<UserLinksManager> logger) : base(bus, logger)
    {
        Guard.Against.Null(mediator);
        Guard.Against.Null(bus);
        Guard.Against.Null(logger);

        _bus = bus;
        _mediator = mediator;
    }

    /// <summary>
    /// Adds a Link to the AuthUser's collection of Links.
    /// </summary>
    /// <param name="userId">The id of the user to associate the link to</param>
    /// <param name="linkId">The id of the Link itself (not to be confused with the UserLink's Id).</param>
    /// <param name="title">The title that the user would like to display, instead of the underlying one (Optional)</param>
    /// <param name="tags">A collection of tags that the user would like to associate with this link (Optional)</param>
    /// <param name="isPrivate">Whether or not the user would like to publicly display this link in their list</param>
    /// <param name="token"></param>
    /// <returns>The id of the newly created UserLink</returns>
    public async Task<string> AddLinkAsync(string userId, string linkId, string title = "", string[]? tags = default, bool isPrivate = false, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(userId);
        Guard.Against.NullOrWhiteSpace(linkId);

        var command = new AddLinkToUserCommand(userId, linkId, title, tags, isPrivate);

        var newId = await _mediator.Send(command, token);

        return newId.ToString();
    }

    public Task<UserLink?> GetUserLinkAsync(string userId, string linkId, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(userId);
        Guard.Against.NullOrWhiteSpace(linkId);

        var query = new GetUserLinkByIdQuery(userId, linkId);

        // TODO: Get Link from central repo and merge their details with AuthUser's Link
        // MergeLinks
        return _mediator.Send(query, token);
    }

    public async Task<PagedResults<UserLink>> GetUserLinksAsync(string userId, int pageNo, int pageSize, CancellationToken token = default)
    {
        Guard.Against.NullOrWhiteSpace(userId);

        var queryUserLinks = new GetUserLinksQuery(userId, pageNo, pageSize);
        var userLinks = await _mediator.Send(queryUserLinks, token);

        if (!userLinks.Results.Any())
            return new PagedResults<UserLink>(new List<UserLink>(), 0, 0, 0);

        var linkIds = userLinks.Results.Select(l => l.LinkId).ToList();

        var queryLinks = new GetLinksByIdsQuery(linkIds.Select(Guid.Parse).ToArray());
        var links = await _mediator.Send(queryLinks, token);

        var updatedUserLinks = MergeLinks(userLinks.Results, links).ToList();

        userLinks.Results = updatedUserLinks;

        return userLinks;
    }

    /// <summary>
    /// Merges the AuthUser's Links with the original links, using the AuthUser's version of it's details if they exist.
    /// </summary>
    /// <param name="userLinks">A collection of links that belong to the user</param>
    /// <param name="baseLinks">A collection of the original links (as LinkItems)</param>
    /// <returns>The user's links that have been updated with the original link's information, where applicable</returns>
    private IEnumerable<UserLink> MergeLinks(IEnumerable<UserLink> userLinks, IEnumerable<LinkItem> baseLinks)
    {
        // I drew a blank here coming up with names for these.
        var userItems = userLinks.ToList();
        var originalItems = baseLinks.ToList();

        Parallel.ForEach(userItems, new ParallelOptions { MaxDegreeOfParallelism = 5 },
            item1 =>
        {
            var item2 = originalItems.Find(l => l.Id == item1.LinkId);

            if (item2 != null)
            {
                // If the user's link has information, then use its version, if not, then use the original's
                item1.Title = !string.IsNullOrWhiteSpace(item1.Title) ? item1.Title : item2.Title;
            }
        });

        // Original version
        //foreach (var item1 in items1)
        //{
        //    var item2 = items2.Find(l => l.Id == item1.Id);

        //    if (string.IsNullOrWhiteSpace(item2?.Title))
        //        continue;

        //    // If the user's link has information, then use its version, if not, then use the original's
        //    item1.Title = !string.IsNullOrWhiteSpace(item1.Title) ? item1.Title : item2.Title;
        //    //item1.Description = (!string.IsNullOrWhiteSpace(item1.Description) ? item1.Description : item2.Description) ?? string.Empty;
        //}

        return userItems;
    }


}