using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a single link from the central repository, by the link's exact url
/// </summary>
public class GetLinkByUrlQueryHandler : IRequestHandler<GetLinkByUrlQuery, LinkDto?>
{
    private readonly ILinksService _linksService;

    public GetLinkByUrlQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<LinkDto?> Handle(GetLinkByUrlQuery command, CancellationToken cancellationToken)
    {
        var link = await _linksService.GetByUrlAsync(command.Url, cancellationToken);

        return link;
    }
}