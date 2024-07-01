using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Common.Interfaces;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries.Handlers;

public class GetLinksRelatedTagsQueryHandler : IRequestHandler<GetLinksRelatedTagsQuery, LinkTagDto[]>
{
    private readonly ILinksService _linksService;

    public GetLinksRelatedTagsQueryHandler(ILinksService linksService)
    {
        Guard.Against.Null(linksService);

        _linksService = linksService;
    }

    public async Task<LinkTagDto[]> Handle(GetLinksRelatedTagsQuery command, CancellationToken cancellationToken)
    {
        var results = (await _linksService.GetRelatedTagsAsync(command.Tags, command.Count.GetValueOrDefault(), token: cancellationToken)).ToArray();

        return results;
    }
}