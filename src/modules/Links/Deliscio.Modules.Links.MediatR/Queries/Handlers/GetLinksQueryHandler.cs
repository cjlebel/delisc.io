using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class GetLinksQueryHandler : IRequestHandler<GetLinksQuery, PagedResults<Link>>
{
    private readonly ILinksService _linksService;

    public GetLinksQueryHandler(ILinksService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<PagedResults<Link>> Handle(GetLinksQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetAsync(command.PageNo, command.PageSize, token: cancellationToken);

        return results;
    }
}