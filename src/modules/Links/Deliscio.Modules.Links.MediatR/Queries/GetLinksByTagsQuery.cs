using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

//TODO: This should be moved to GetLinksQuery
[Obsolete("Use GetLinksQuery")]
public record GetLinksByTagsQuery : IRequest<PagedResults<LinkItem>>
{
    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public string[] Tags { get; init; }

    public GetLinksByTagsQuery(int pageNo, int pageSize, string[] tags)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        Tags = tags;

        PageNo = pageNo;
        PageSize = pageSize;
    }
}