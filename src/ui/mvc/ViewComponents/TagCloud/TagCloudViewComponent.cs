using Deliscio.Modules.Links.MediatR.Queries;
using Deliscio.Web.Mvc.Models.Tags;
using Microsoft.AspNetCore.Mvc;
using IMediator = MediatR.IMediator;

namespace Deliscio.Web.Mvc.ViewComponents.TagCloud;

public class TagCloudViewComponent : ViewComponent
{
    private readonly IMediator _mediator;

    public TagCloudViewComponent(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IViewComponentResult> InvokeAsync(string[] tags, string title = "", int count = 50)
    {
        var command = new GetLinksRelatedTagsQuery(new string[] { }, count: count);

        var newTags = await _mediator.Send(command);

        var model = new TagsModel(newTags, title);

        return View(model);
    }
}