using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public record GetRelatedTagsByTagsQuery : IRequest<LinkTag[]>
{
    public int? Count { get; init; }
    public string[] Tags { get; init; }

    public GetRelatedTagsByTagsQuery(string[]? tags = default, int? count = 100)
    {
        Count = count;
        Tags = tags ?? Array.Empty<string>();
    }
}