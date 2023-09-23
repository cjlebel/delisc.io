using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Handlers.Queries;

/// <summary>
/// Handles getting a page of links from the central repository where the tags belong to the domain specified.
/// </summary>
public class GetLinkByDomainQueryHandler : IRequestHandler<GetLinksByDomainQuery, PagedResults<Link>>
{
    private readonly ILinksService _linksService;

    public GetLinkByDomainQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<PagedResults<Link>> Handle(GetLinksByDomainQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetByDomain(command.Domain, command.PageNo, command.PageSize, token: cancellationToken);

        return results;
    }
}