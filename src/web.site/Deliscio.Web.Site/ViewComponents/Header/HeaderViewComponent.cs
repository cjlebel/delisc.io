using Deliscio.Web.Site.ViewComponents.TagCloud;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Site.ViewComponents.Header;

public class HeaderViewComponent : ViewComponent
{
    private readonly IMediator _mediator;

    public HeaderViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}