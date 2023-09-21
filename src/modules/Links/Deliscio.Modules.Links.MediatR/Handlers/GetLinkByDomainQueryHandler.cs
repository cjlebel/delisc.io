using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Queries;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Handlers;

/// <summary>
/// Handles getting a page of links from the central repository where the tags belong to the domain specified.
/// </summary>
public class GetLinkByDomainQueryHandler : IRequestHandler<GetLinksByDomainQuery, PagedResults<Link>>
{
    private readonly LinksService _linksService;

    public GetLinkByDomainQueryHandler(LinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<PagedResults<Link>> Handle(GetLinksByDomainQuery request, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetByDomain(request.Domain, request.PageNo, request.PageSize, token: cancellationToken);

        return results;
    }
}