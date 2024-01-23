using Deliscio.Core.Models;

namespace Deliscio.Web.Mvc.ViewModels;

public record BasePageViewModel
{
    public string CanonicalUrl { get; init; } = string.Empty;

    public string PageTitle { get; init; } = string.Empty;

    public string PageDescription { get; init; } = string.Empty;
}

public record BasePagePagedViewModel<T> : BasePageViewModel
{
    public PagedResults<T>? Results { get; init; } = new();
}