using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Queries.GetLinkByUrl;

public sealed class GGetLinkByUrlQueryHandler(ILinksRepository linksRepository)
    : IRequestHandler<GetLinkByUrlQuery, Result<LinkDto>>
{
    public async Task<Result<LinkDto>> Handle(GetLinkByUrlQuery query, CancellationToken cancellationToken)
    {
        var results = await linksRepository.GetLinkByIdAsync(query.Id, cancellationToken);

        if (results.IsFailed)
            return Result.Fail(results.Errors);

        var link = (LinkDto)results.Value;

        return Result.Ok(link);
    }
}
