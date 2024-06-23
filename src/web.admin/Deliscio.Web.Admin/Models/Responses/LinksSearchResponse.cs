using Deliscio.Core.Models;

namespace Deliscio.Web.Admin.Models.Responses;

public class LinksSearchResponse<T>
{
    public string SearchTerm { get; set; } = "";

    public string Domain { get; set; } = "";

    public string[] Tags { get; set; } = Array.Empty<string>();

    public bool IsActive { get; set; } = true;

    public bool IsFlagged { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    public PagedResults<T> Results { get; set; } = new PagedResults<T>();

    public static LinksSearchResponse<T> Create(PagedResults<T> pagedResults, string searchTerm, string domain, string tags,
        bool isActive, bool isFlagged, bool isDeleted)
    {
        return new LinksSearchResponse<T>
        {
            SearchTerm = searchTerm,
            Domain = domain,
            Tags = tags?.Split(',') ?? [],

            Results = pagedResults,

            IsActive = isActive,
            IsFlagged = isFlagged,
            IsDeleted = isDeleted
        };
    }
}