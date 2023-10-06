using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetLinksQuery : IRequest<PagedResults<LinkItem>>
{
    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public GetLinksQuery(int pageNo, int pageSize)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        PageNo = pageNo;
        PageSize = pageSize;
    }
}