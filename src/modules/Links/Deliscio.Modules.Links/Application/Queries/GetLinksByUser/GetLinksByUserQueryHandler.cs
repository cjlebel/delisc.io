using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Queries.GetLinksByUser;

public sealed class GetLinkByIdQueryHandler(ILinksRepository linksRepository)
    : IRequestHandler<GetLinksByUserQuery, Result<IEnumerable<LinkDto>>>
{
    public async Task<Result<IEnumerable<LinkDto>>> Handle(GetLinksByUserQuery query, CancellationToken cancellationToken)
    {
        var results = await linksRepository.GetLinksByUserAsync(query.UserId, cancellationToken);

        if (results.IsFailed)
            return Result.Fail(results.Errors);

        var links = results.Value.Select(x => (LinkDto)x);

        return Result.Ok(links);
    }
}
