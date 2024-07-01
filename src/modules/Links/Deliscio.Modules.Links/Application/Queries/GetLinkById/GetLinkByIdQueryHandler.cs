using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Queries.GetLinkById;

public sealed class GetLinkByIdQueryHandler(ILinksRepository linksRepository)
    : IRequestHandler<GetLinkByIdQuery, Result<LinkDto>>
{
    public async Task<Result<LinkDto>> Handle(GetLinkByIdQuery query, CancellationToken cancellationToken)
    {
        var results = await linksRepository.GetLinkByIdAsync(query.Id, cancellationToken);

        if (results.IsFailed)
            return Result.Fail(results.Errors);

        var link = (LinkDto)results.Value;

        return Result.Ok(link);
    }
}
