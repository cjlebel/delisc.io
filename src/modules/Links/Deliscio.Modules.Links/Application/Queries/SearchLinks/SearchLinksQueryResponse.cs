using Deliscio.Core.Models;
using Deliscio.Modules.Links.Application.Dtos;

namespace Deliscio.Modules.Links.Application.Queries.SearchLinks;

/// <summary>
/// Represents the response from doing a search for the links
/// </summary>
public class SearchLinksQueryResponse
{
    public string SearchTerm { get; set; } = "";

    public string Domain { get; set; } = "";

    public string[] Tags { get; set; } = Array.Empty<string>();

    public bool? IsActive { get; set; } = true;

    public bool? IsFlagged { get; set; } = false;

    public bool? IsDeleted { get; set; } = false;

    public PagedResults<LinkDto> Results { get; set; } = new PagedResults<LinkDto>();

    public static SearchLinksQueryResponse Create(IEnumerable<LinkDto> items, 
        int pageNo, 
        int pageSize, 
        int skip, 
        int totalResults, 
        string searchTerm, 
        string domain, 
        string tags,
        bool? isActive, bool? isFlagged, bool? isDeleted)
    {
        return Create(new PagedResults<LinkDto>(items, pageNo, pageSize, totalResults, offset: 0), 
            searchTerm, 
            domain, 
            tags,
            isActive, 
            isFlagged, 
            isDeleted
        );
    }

    public static SearchLinksQueryResponse Create(IEnumerable<LinkDto> items,
        int pageNo,
        int pageSize,
        int skip,
        int totalResults,
        string searchTerm,
        string domain,
        string[] tags,
        bool? isActive, bool? isFlagged, bool? isDeleted)
    {
        return Create(new PagedResults<LinkDto>(items, pageNo, pageSize, totalResults, offset: skip),
            searchTerm, 
            domain, 
            tags, 
            isActive, 
            isFlagged, 
            isDeleted);

        //return new SearchLinksQueryResponse<T>
        //{
        //    SearchTerm = searchTerm,
        //    Domain = domain,
        //    TagsCollection = tags ?? [],

        //    Results = new PagedResults<T>(items, pageNo, pageSize, skip, totalResults),

        //    IsActive = isActive,
        //    IsFlagged = isFlagged,
        //    IsDeleted = isDeleted
        //};
    }

    public static SearchLinksQueryResponse Create(PagedResults<LinkDto> pagedResults, string searchTerm, string domain, string tags,
        bool? isActive, bool? isFlagged, bool? isDeleted)
    {
        return Create(pagedResults, searchTerm, domain, tags.Split(',').Select(t=>t.Trim().ToLower()).ToArray(), isActive, isFlagged, isDeleted);
    }

    public static SearchLinksQueryResponse Create(PagedResults<LinkDto> pagedResults, string searchTerm, string domain, string[] tags,
        bool? isActive, bool? isFlagged, bool? isDeleted)
    {
        return new SearchLinksQueryResponse
        {
            SearchTerm = searchTerm,
            Domain = domain,
            Tags = tags ?? [],

            Results = pagedResults,

            IsActive = isActive,
            IsFlagged = isFlagged,
            IsDeleted = isDeleted
        };
    }
}