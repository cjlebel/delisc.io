using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.MediatR.Queries;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Handlers.Queries;

/// <summary>
/// Handles getting a page of links from the central repository where each link contains all of the tags
/// </summary>
public class GetLinkByTagsQueryHandler : IRequestHandler<GetLinksByTagsQuery, PagedResults<Link>>
{
    private readonly ILinksService _linksService;

    public GetLinkByTagsQueryHandler(ILinksService linksService)
    {
        _linksService = linksService;
    }

    public async Task<PagedResults<Link>> Handle(GetLinksByTagsQuery command, CancellationToken cancellationToken)
    {
        var results = await _linksService.GetByTags(command.Tags, command.PageNo, command.PageSize, token: cancellationToken);

        return results;
    }
}