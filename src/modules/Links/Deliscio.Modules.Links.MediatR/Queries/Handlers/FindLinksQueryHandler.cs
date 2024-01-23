using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class FindLinksQueryHandler : IRequestHandler<FindLinksQuery, PagedResults<LinkItem>>
{
    private readonly ILinksService _linksService;

    public FindLinksQueryHandler(ILinksService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<PagedResults<LinkItem>> Handle(FindLinksQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.FindAsync(command.SearchTerm, command.PageNo, command.PageSize, token: cancellationToken);

        return results;
    }
}