using Deliscio.Core.Models;
using Deliscio.Modules.Links.Application.Dtos;
using Deliscio.Modules.Links.Infrastructure.Data;
using FluentResults;
using MediatR;

namespace Deliscio.Modules.Links.Application.Queries.SearchLinks;

public sealed class SearchLinksQueryHandler(ILinksRepository linksRepository)
    : IRequestHandler<SearchLinksQuery, Result<SearchLinksQueryResponse>>
{
    public async Task<Result<SearchLinksQueryResponse>> Handle(SearchLinksQuery query, CancellationToken cancellationToken)
    {
        var results = await linksRepository.FindLinksAsync(
            query.Term,
            query.Tags,
            query.Domain,
            query.PageNo,
            query.PageSize,
            query.Offset,
            query.IsActive,
            query.IsFlagged,
            query.IsDeleted,
            cancellationToken
        );

        if (results.IsFailed)
        {
            return Result.Fail(results.Errors);
        }

        var value = results.Value;
        var searchLinkItems = value.Results.Select(l => (LinkDto)l)?.ToArray() ?? [];

        //var pagedResults = new PagedResults<SearchLinkItemDto>(searchLinkItems, value.Count, query.PageNo, query.PageSize);

        var response = SearchLinksQueryResponse.Create(
            searchLinkItems, 
            query.PageNo, 
            query.PageSize, 
            query.Offset, 
            value.TotalCount, 
            query.Term, 
            query.Domain, 
            query.Tags, 
            query.IsActive, 
            query.IsFlagged, 
            query.IsDeleted
        );

        return Result.Ok(response);
    }
}
