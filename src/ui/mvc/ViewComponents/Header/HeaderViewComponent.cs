using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Mvc.ViewComponents.Header;

public class HeaderViewComponent : ViewComponent
{
    public HeaderViewComponent()
    {
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}