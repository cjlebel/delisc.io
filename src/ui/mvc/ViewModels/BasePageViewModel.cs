using Ardalis.GuardClauses;
using Deliscio.Core.Models;

namespace Deliscio.Web.Mvc.ViewModels;

public record BasePageViewModel
{
    public string CanonicalUrl { get; init; } = string.Empty;

    public string PageTitle { get; init; } = string.Empty;

    public string PageDescription { get; init; } = string.Empty;
}

public record BasePagePagedViewModel<T> : PagedResults<T>
{
    public string CanonicalUrl { get; init; } = string.Empty;

    public string PageTitle { get; init; } = string.Empty;

    public string PageDescription { get; init; } = string.Empty;

    public BasePagePagedViewModel(PagedResults<T> results) : base(results)
    {
        Guard.Against.Null(results);

        Results = results.Results;
        PageNumber = results.PageNumber;
        PageSize = results.PageSize;
        TotalResults = results.TotalResults;
    }
}