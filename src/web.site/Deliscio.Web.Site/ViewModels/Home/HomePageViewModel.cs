using Deliscio.Core.Models;
using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Web.Site.ViewModels.Home;

public record HomePageViewModel : BasePagePagedViewModel<LinkItem>
{
    public HomePageViewModel(PagedResults<LinkItem> results) : base(results) { }
}