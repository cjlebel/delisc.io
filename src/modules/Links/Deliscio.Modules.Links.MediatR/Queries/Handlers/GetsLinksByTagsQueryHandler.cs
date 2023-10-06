using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;

using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class GetsLinksByTagsQueryHandler : IRequestHandler<GetLinksByTagsQuery, PagedResults<LinkItem>>
{
    private readonly ILinksService _linksService;

    public GetsLinksByTagsQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<PagedResults<LinkItem>> Handle(GetLinksByTagsQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetLinksByTagsAsync(command.Tags, command.PageNo, command.PageSize, token: cancellationToken);

        return results;
    }
}