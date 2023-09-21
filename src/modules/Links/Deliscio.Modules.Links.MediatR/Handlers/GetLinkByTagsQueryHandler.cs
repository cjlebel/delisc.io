using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Common.Queries;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Handlers;

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class GetLinkByTagsQueryHandler : IRequestHandler<GetLinksByTagsQuery, PagedResults<Link>>
{
    private readonly LinksService _linksService;

    public GetLinkByTagsQueryHandler(LinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<PagedResults<Link>> Handle(GetLinksByTagsQuery request, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetByTags(request.Tags, request.PageNo, request.PageSize, token: cancellationToken);

        return results;
    }
}