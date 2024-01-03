using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

public class GetRelatedTagsByTagsQueryHandler : IRequestHandler<GetRelatedTagsByTagsQuery, LinkTag[]>
{
    private readonly ILinksService _linksService;

    public GetRelatedTagsByTagsQueryHandler(ILinksService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<LinkTag[]> Handle(GetRelatedTagsByTagsQuery command, CancellationToken cancellationToken)
    {
        var results = (await _linksService.GetRelatedTagsAsync(command.Tags, command.Count.GetValueOrDefault(), token: cancellationToken)).ToArray();

        return results;
    }
}