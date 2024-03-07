using Deliscio.Core.Models;

namespace Deliscio.Admin.Models.Responses;

public record LinksSearchResponse<T> : PagedResults<T> where T : class
{
    public string SearchTerm { get; set; } = "";

    public string Domain { get; set; } = "";

    public string[] Tags { get; set; } = Array.Empty<string>();

    public bool IsActive { get; set; } = true;

    public bool IsFlagged { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    public static LinksSearchResponse<T> Create(PagedResults<T> pagedResults, string searchTerm, string domain, string tags,
        bool isActive, bool isFlagged, bool isDeleted)
    {
        return new LinksSearchResponse<T>
        {
            SearchTerm = searchTerm,
            Domain = domain,
            Tags = tags?.Split(',') ?? [],

            PageNumber = pagedResults.PageNumber,
            PageSize = pagedResults.PageSize,
            TotalResults = pagedResults.TotalResults,

            Results = pagedResults.Results,

            IsActive = isActive,
            IsFlagged = isFlagged,
            IsDeleted = isDeleted
        };
    }
}