using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deliscio.Web.Site.ViewComponents.TagCloud;

public class ViewComponentBase : ViewComponent
{
    protected IMediator Mediator;

    public ViewComponentBase(IMediator mediator)
    {
        Mediator = mediator;
    }

    public virtual Task<IViewComponentResult> InvokeAsync()
    {
        throw new ArgumentException("You must override the InvokeAsync method in your ViewComponent");
    }
}