using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Web.Mvc.ViewModels.Links;

public record LinksPageViewModel : BasePageViewModel
{
    public PagedResults<LinkItem> Links { get; init; } = new();

    public string[] Tags { get; init; } = Array.Empty<string>();
}