using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles executing the GetLinksByIds Query and getting a collection of links by their unique id
/// </summary>
public class GetLinksByIdsQueryHandler : IRequestHandler<GetLinksByIdsQuery, IEnumerable<Link>>
{
    private readonly ILinksService _linksService;

    public GetLinksByIdsQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<IEnumerable<Link>> Handle(GetLinksByIdsQuery command, CancellationToken cancellationToken)
    {
        var link = await _linksService.GetByIdsAsync(command.Ids, cancellationToken);

        return link;
    }
}