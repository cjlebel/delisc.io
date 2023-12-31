using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Web.Mvc.ViewModels.Home;

public record HomePageViewModel : BasePageViewModel
{
    public PagedResults<LinkItem> Links { get; init; } = new();
}