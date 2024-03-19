using Amazon.Runtime.Internal.Transform;
using Ardalis.GuardClauses;

namespace Deliscio.Modules.Links.Common.Models.Requests;

/// <summary>
/// A request object that represents a query for finding links that are active and publicly viewable
/// </summary>
public record FindLinksRequest
{
    public string Domain { get; init; }

    public string SearchTerm { get; init; }

    public string[] Tags { get; init; }


    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public int Skip { get; init; }


    public bool? IsActive => true;

    public bool? IsFlagged => null;

    public bool? IsDeleted => false;

    public FindLinksRequest(int pageNo = 1, string term = "", string domain = "", string tags = "", int pageSize = 50, int skip = 0) : this(pageNo, term, domain, tags?.Trim().Split(','), pageSize, skip) { }

    public FindLinksRequest(int pageNo = 1, string term = "", string domain = "", string[]? tags = default, int pageSize = 50, int skip = 0)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);
        Guard.Against.Negative(skip);

        Domain = domain;
        SearchTerm = term;
        Tags = tags?.OrderBy(t => t).ToArray() ?? [];

        PageNo = pageNo;
        PageSize = pageSize;
        Skip = skip;
    }
}

/// <summary>
/// A request object that represents a query for finding any and all links
/// </summary>
public record FindLinksAdminRequest
{
    public string Domain { get; init; }

    public string SearchTerm { get; init; }

    public string[] Tags { get; init; }


    public int PageNo { get; init; }

    public int PageSize { get; init; }

    public int Skip { get; init; }



    public bool? IsActive { get; init; }

    public bool? IsFlagged { get; init; }

    public bool? IsDeleted { get; init; }

    public FindLinksAdminRequest(int pageNo = 1, string term = "", string domain = "", string tags = "", int pageSize = 50, int skip = 0, bool? isActive = default, bool? isFlagged = true, bool? isDeleted = false) : this(pageNo, term, domain, tags?.Trim().Split(','), pageSize, skip, isActive, isFlagged, isDeleted) { }

    public FindLinksAdminRequest(int pageNo = 1, string term = "", string domain = "", string[]? tags = default, int pageSize = 50, int skip = 0, bool? isActive = default, bool? isFlagged = true, bool? isDeleted = false)
    {
        Guard.Against.NegativeOrZero(pageNo);
        Guard.Against.NegativeOrZero(pageSize);
        Guard.Against.Negative(skip);

        Domain = domain;
        SearchTerm = term;
        Tags = tags?.OrderBy(t => t).ToArray() ?? [];

        PageNo = pageNo;
        PageSize = pageSize;
        Skip = skip;

        IsActive = isActive;
        IsFlagged = isFlagged;
        IsDeleted = isDeleted;
    }
}