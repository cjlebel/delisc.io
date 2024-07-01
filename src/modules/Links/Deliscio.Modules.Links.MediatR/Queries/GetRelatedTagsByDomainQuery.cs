using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Application.Dtos;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public record GetRelatedTagsByDomainQuery : IRequest<LinkTagDto[]>
{
    public int? Count { get; init; }
    public string Domain { get; init; }

    public GetRelatedTagsByDomainQuery(string domain, int? count = default)
    {
        Guard.Against.NullOrWhiteSpace(domain);

        Count = count;
        Domain = domain;
    }
}