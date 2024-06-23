using Deliscio.Modules.Links.Application.Dtos;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public record GetLinksRelatedTagsQuery : IRequest<LinkTagDto[]>
{
    public int? Count { get; init; }
    public string[] Tags { get; init; }

    public GetLinksRelatedTagsQuery(string[] tags, int? count = default)
    {
        Count = count;
        Tags = tags;
    }
}