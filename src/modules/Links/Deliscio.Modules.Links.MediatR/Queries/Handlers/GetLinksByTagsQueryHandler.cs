using Deliscio.Core.Models;
using Deliscio.Modules.Links.Application.Dtos;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class GetLinksByTagsQueryHandler : IRequestHandler<GetLinksByTagsQuery, PagedResults<LinkItemDto>>
{
    private readonly ILinksService _linksService;

    public GetLinksByTagsQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<PagedResults<LinkItemDto>> Handle(GetLinksByTagsQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetLinksByTagsAsync(command.Tags, command.PageNo, command.PageSize, token: cancellationToken);

        return results;
    }
}