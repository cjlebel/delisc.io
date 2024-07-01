using Deliscio.Core.Models;
using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a page of links from the central repository where the tags belong to the domain specified.
/// </summary>
public class GetLinksByDomainQueryHandler : IRequestHandler<GetLinksByDomainQuery, PagedResults<LinkItemDto>>
{
    private readonly ILinksService _linksService;

    public GetLinksByDomainQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<PagedResults<LinkItemDto>> Handle(GetLinksByDomainQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetLinksByDomainAsync(command.Domain, command.PageNo, command.PageSize, token: cancellationToken);

        return results;
    }
}