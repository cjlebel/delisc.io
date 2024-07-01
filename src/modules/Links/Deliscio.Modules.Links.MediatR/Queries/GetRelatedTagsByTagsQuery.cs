using Deliscio.Modules.Links.Application.Dtos;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public record GetRelatedTagsByTagsQuery : IRequest<LinkTagDto[]>
{
    public int? Count { get; init; }
    public string[] Tags { get; init; }

    public GetRelatedTagsByTagsQuery(string? tags = default, int? count = 100)
    {
        Count = count;
        Tags = !string.IsNullOrWhiteSpace(tags) ?
            tags.Split(',').OrderBy(t => t).ToArray() :
            Array.Empty<string>();
    }
}