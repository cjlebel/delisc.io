using Microsoft.AspNetCore.Authorization;

namespace Deliscio.Modules.Links.Requests;

/// <summary>
/// A request object that represents a query for finding links that are active and publicly viewable
/// </summary>
public record FindLinksRequest
{
    public string Domain { get; init; } = "";

    public string SearchTerm { get; init; } = "";

    public string[] Tags { get; init; } = [];


    public int PageNo { get; init; } = 1;

    public int PageSize { get; init; } = 50;

    public int Offset { get; init; } = 0;


    public bool? IsActive { get; private set; } = true;

    public bool? IsFlagged { get; private set; } = null;

    public bool? IsDeleted { get; private set; } = false;


    private FindLinksRequest() { }

    /// <summary>
    /// CreateForAdmin a new FindLinksRequest that will only search for publicly available links
    /// </summary>
    /// <param name="pageNo">The page number of results to be returned</param>
    /// <param name="pageSize">The max size of the page of results</param>
    /// <param name="term">The term to search for</param>
    /// <param name="domain">The domain that the links belong to</param>
    /// <param name="tags">Any tags to search for that belong to the links to be returned</param>
    /// <returns>A FindLinksRequest that will return active links</returns>
    public static FindLinksRequest Create(int pageNo = 1, int pageSize = 50, int offset = 0, 
        string term = "", string domain = "", string tags = "")
    {
        return Create(true, null, false, pageNo, pageSize, offset, term, domain, tags);
    }

    /// <summary>
    /// Creates a new FindLinksRequest with unfettered access to ALL links
    /// </summary>
    /// <param name="isActive">Active/Inactive/All</param>
    /// <param name="isFlagged">Flagged/Not Flagged/All</param>
    /// <param name="isDeleted">Deleted/Not Deleted/All</param>
    /// <param name="pageNo">The page number of results to be returned</param>
    /// <param name="pageSize">The max size of the page of results</param>
    /// <param name="offset"></param>
    /// <param name="term">The term to search for</param>
    /// <param name="domain">The domain that the links belong to</param>
    /// <param name="tags">Any tags to search for that belong to the links to be returned</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    public static FindLinksRequest Create(bool? isActive, bool? isFlagged, bool? isDeleted, 
        int pageNo = 1, int pageSize = 50, int offset = 0, 
        string term = "", string domain = "", string tags = "")
    {
        return new FindLinksRequest
        {
            SearchTerm = term,
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



    //public FindLinksRequest(int pageNo = 1, string term = "", string domain = "", string tags = "", int pageSize = 50, int skip = 0) : this(pageNo, term, domain, tags?.Trim().Split(','), pageSize, skip) { }

    //public FindLinksRequest(int pageNo = 1, string term = "", string domain = "", string[]? tags = default, int pageSize = 50, int skip = 0)
    //{
    //    Guard.Against.NegativeOrZero(pageNo);
    //    Guard.Against.NegativeOrZero(pageSize);
    //    Guard.Against.Negative(skip);

    //    Domain = domain;
    //    Term = term;
    //    TagsCollection = tags?.OrderBy(t => t).ToArray() ?? [];

    //    PageNo = pageNo;
    //    PageSize = pageSize;
    //    Offset = skip;
    //}
}

/// <summary>
/// A request object that represents a query for finding any and all links
/// </summary>
//public record FindLinksAdminRequest
//{
//    public string Domain { get; init; }

//    public string Term { get; init; }

//    public string[] TagsCollection { get; init; }


//    public int PageNo { get; init; }

//    public int PageSize { get; init; }

//    public int Offset { get; init; }



//    public bool? IsActive { get; init; }

//    public bool? IsFlagged { get; init; }

//    public bool? IsDeleted { get; init; }

//    public FindLinksAdminRequest(int pageNo = 1, string term = "", string domain = "", string tags = "", int pageSize = 50, int skip = 0, bool? isActive = default, bool? isFlagged = true, bool? isDeleted = false) : this(pageNo, term, domain, tags?.Trim().Split(','), pageSize, skip, isActive, isFlagged, isDeleted) { }

//    public FindLinksAdminRequest(int pageNo = 1, string term = "", string domain = "", string[]? tags = default, int pageSize = 50, int skip = 0, bool? isActive = default, bool? isFlagged = true, bool? isDeleted = false)
//    {
//        Guard.Against.NegativeOrZero(pageNo);
//        Guard.Against.NegativeOrZero(pageSize);
//        Guard.Against.Negative(skip);

//        Domain = domain;
//        Term = term;
//        TagsCollection = tags?.OrderBy(t => t).ToArray() ?? [];

//        PageNo = pageNo;
//        PageSize = pageSize;
//        Offset = skip;

//        IsActive = isActive;
//        IsFlagged = isFlagged;
//        IsDeleted = isDeleted;
//    }
//}