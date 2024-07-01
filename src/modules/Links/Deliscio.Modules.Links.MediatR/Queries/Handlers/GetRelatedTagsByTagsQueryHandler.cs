using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

public class GetRelatedTagsByTagsQueryHandler : IRequestHandler<GetRelatedTagsByTagsQuery, LinkTagDto[]>
{
    private readonly ILinksService _linksService;

    public GetRelatedTagsByTagsQueryHandler(ILinksService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<LinkTagDto[]> Handle(GetRelatedTagsByTagsQuery command, CancellationToken cancellationToken)
    {
        var results = (await _linksService.GetRelatedTagsAsync(command.Tags, command.Count.GetValueOrDefault(), token: cancellationToken)).ToArray();

        return results;
    }
}