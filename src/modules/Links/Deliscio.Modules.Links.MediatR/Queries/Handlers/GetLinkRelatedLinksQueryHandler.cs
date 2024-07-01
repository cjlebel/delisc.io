using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a single link from the central repository, by the link's id
/// </summary>
public class GetLinkRelatedLinksQueryHandler : IRequestHandler<GetLinkRelatedLinksQuery, LinkItemDto[]>
{
    private readonly ILinksService _linksService;

    public GetLinkRelatedLinksQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<LinkItemDto[]> Handle(GetLinkRelatedLinksQuery command, CancellationToken cancellationToken)
    {
        var link = await _linksService.GetRelatedLinksAsync(command.Id, null, cancellationToken);

        return link;
    }
}