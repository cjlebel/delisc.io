using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Application.Dtos;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public record GetLinksByDomainQuery : IRequest<PagedResults<LinkItemDto>>
{
    public string Domain { get; init; }
    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public GetLinksByDomainQuery(string domain, int pageNo, int pageSize)
    {
        Guard.Against.NullOrEmpty(domain);
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);

        Domain = domain;

        PageNo = pageNo;
        PageSize = pageSize;
    }
}