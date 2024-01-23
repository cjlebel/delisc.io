using Ardalis.GuardClauses;
using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;
using MediatR;

namespace Deliscio.Modules.Links.MediatR.Queries;

public sealed record FindLinksQuery : IRequest<PagedResults<LinkItem>>
{
    /// <summary>
    /// The current page number of results
    /// </summary>
    public int PageNo { get; init; } = 1;

    /// <summary>
    /// The size of the collection to be returned
    /// </summary>
    public int PageSize { get; init; } = 50;

    public string SearchTerm { get; init; } = string.Empty;

    /// <summary>
    /// The number of items to skip before returning results
    /// </summary>
    public int Skip { get; init; } = 0;

    public string[] Tags { get; init; }

    public FindLinksQuery(string search, int pageNo, int pageSize, int? skip = 0, string? tags = "")
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);
        Guard.Against.Negative(skip.GetValueOrDefault());

        PageNo = pageNo;
        PageSize = pageSize;
        SearchTerm = search;

        Tags = !string.IsNullOrWhiteSpace(tags) ?
            tags.Split(',').OrderBy(t => t).ToArray() :
            Array.Empty<string>();
    }
}