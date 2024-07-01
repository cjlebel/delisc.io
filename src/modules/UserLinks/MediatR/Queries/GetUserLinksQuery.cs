using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.UserLinks.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record GetUserLinksQuery : IRequest<PagedResults<UserLink>>
{
    public string UserId { get; init; }
    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public GetUserLinksQuery(string userId, int pageNo, int pageSize)
    {
        Guard.Against.NullOrWhiteSpace(userId);
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        UserId = userId;

        PageNo = pageNo;
        PageSize = pageSize;
    }
}