using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a single link from the central repository, by the link's id
/// </summary>
public class GetLinkRelatedLinksQueryHandler : IRequestHandler<GetLinkRelatedLinksQuery, LinkItem[]>
{
    private readonly ILinksService _linksService;

    public GetLinkRelatedLinksQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<LinkItem[]> Handle(GetLinkRelatedLinksQuery command, CancellationToken cancellationToken)
    {
        var link = await _linksService.GetRelatedLinksAsync(command.Id, null, cancellationToken);

        return link;
    }
}