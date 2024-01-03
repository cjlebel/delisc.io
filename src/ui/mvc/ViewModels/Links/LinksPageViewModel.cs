using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Web.Mvc.ViewModels.Links;

public record LinksPageViewModel : BasePagePagedViewModel<LinkItem>
{
    public string[] Tags { get; init; } = Array.Empty<string>();
}