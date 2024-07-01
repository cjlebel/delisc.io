using Deliscio.Modules.Links.Application.Contracts;
using FluentResults;

namespace Deliscio.Modules.Links.Application.Queries.SearchLinks;
public sealed record SearchLinksQuery : ILinksQuery<Result<SearchLinksQueryResponse>>
{
    public string Term { get; init; } = string.Empty;

    public string[] Tags { get; init; } = [];

    /// <summary>
    /// The domain that the links to be returned should belong to
    /// </summary>
    public string Domain { get; init; } = string.Empty;

    /// <summary>
    /// Whether to include active items in the search results or not
    /// True = Include active items
    /// False = Exclude active items
    /// Null = Include both active and inactive items
    /// Default is true
    /// </summary>
    public bool? IsActive { get; init; } = true;

    /// <summary>
    /// Whether to include flagged items in the search results or not
    /// True = Include flagged items
    /// False = Exclude flagged items
    /// Null = Include both flagged and non-flagged items
    /// Default is null
    /// </summary>
    public bool? IsFlagged { get; init; } = null;

    /// <summary>
    /// Whether to include deleted items in the search results or not
    /// True = Include deleted items
    /// False = Exclude deleted items
    /// Null = Include both deleted and non-deleted items
    /// Default is false
    /// </summary>
    public bool? IsDeleted { get; init; } = false;

    /// <summary>
    /// The page number of the results to return.
    /// Pages start at 1 and not 0.
    /// </summary>
    public int PageNo { get; init; } = 1;

    /// <summary>
    /// The number of items in the page of results.
    /// </summary>
    public int PageSize { get; init; } = 50;

    /// <summary>
    /// The number of items to offset before returning the first item
    /// Eg: Offset = 10 means that the first result will be the 11th item
    /// This is for when x number of items are already displayed and you want to get the next page of results
    /// </summary>
    public int Offset { get;init; } = 0;

    public static SearchLinksQuery Simple(int pageNo = 1, int pageSize = 50, int offset = 0, string term = "", string domain = "", string tags = "")
    {
        return new SearchLinksQuery
        {
            Term = term,
            Domain = domain,
            Tags = tags?.Trim().Split(',') ?? [],

            PageNo = pageNo,
            PageSize = pageSize,
            Offset = offset,

            IsActive = true,
            IsFlagged = null,
            IsDeleted = false
        };
    }

    /// <summary>
    /// Creates a new FindLinksRequest with unfettered access to ALL links
    /// </summary>
    /// <param name="pageNo">The page number of results to be returned</param>
    /// <param name="pageSize">The max size of the page of results</param>
    /// <param name="offset">The number of items to offset, before paging</param>
    /// <param name="term">The term to search for</param>
    /// <param name="domain">The domain that the links belong to</param>
    /// <param name="tags">Any tags to search for that belong to the links to be returned</param>
    /// <param name="isActive">Active/Inactive/All</param>
    /// <param name="isFlagged">Flagged/Not Flagged/All</param>
    /// <param name="isDeleted">Deleted/Not Deleted/All</param>
    /// <returns></returns>
    public static SearchLinksQuery Advanced(int pageNo = 1, int pageSize = 50, int offset = 0, string term = "", string domain = "", string tags = "", bool? isActive = null, bool? isFlagged = null, bool? isDeleted = null)
    {
        return new SearchLinksQuery
        {
            Term = term,
            Domain = domain,
            Tags = tags?.Trim().Split(',') ?? [],

            PageNo = pageNo,
            PageSize = pageSize,
            Offset = offset,

            IsActive = isActive,
            IsFlagged = isFlagged,
            IsDeleted = isDeleted
        };
    }

}