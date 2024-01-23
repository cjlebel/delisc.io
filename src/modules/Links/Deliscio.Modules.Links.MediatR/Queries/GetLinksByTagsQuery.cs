using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

/// <summary>
/// A specific query to deal with getting links ONLY by tags.
/// This can be used with the tag cloud where no search terms are provided(?)
/// </summary>
public record GetLinksByTagsQuery : IRequest<PagedResults<LinkItem>>
{
    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public string[] Tags { get; init; }

    public GetLinksByTagsQuery(int pageNo, int pageSize, string tags = "")
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        Tags = !string.IsNullOrWhiteSpace(tags) ?
            tags.Split(',').OrderBy(t => t).ToArray() :
            Array.Empty<string>();

        PageNo = pageNo;
        PageSize = pageSize;
    }
}