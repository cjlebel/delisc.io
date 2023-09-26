using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public record GetLinksRelatedTagsQuery : IRequest<LinkTag[]>
{
    public int? Count { get; init; }
    public string[] Tags { get; init; }

    public GetLinksRelatedTagsQuery(string[] tags, int? count = default)
    {
        Guard.Against.NullOrEmpty(tags);

        Count = count;
        Tags = tags;
    }
}