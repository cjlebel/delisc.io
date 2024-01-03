using Deliscio.Web.Mvc.ViewComponents.TagCloud;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Mvc.ViewComponents.Header;

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