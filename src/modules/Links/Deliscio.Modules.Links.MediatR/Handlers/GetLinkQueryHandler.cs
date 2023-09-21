using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Queries;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Handlers;

/// <summary>
/// Handles getting a single link from the central repository, by the link's id
/// </summary>
public class GetLinkByIdQueryHandler : IRequestHandler<GetLinkByIdQuery, Link?>
{
    private readonly LinksService _linksService;

    public GetLinkByIdQueryHandler(LinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<Link?> Handle(GetLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var link = await _linksService.GetAsync(request.Id, cancellationToken);

        return link;
    }
}