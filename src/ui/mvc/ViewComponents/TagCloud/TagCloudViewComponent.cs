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

    public async Task<IViewComponentResult> InvokeAsync(string title = "", int count = 50, string[]? tags = default)
    {
        var q1 = Request.Query["t"];
        var q2 = Request.Query["d"];

        var newTags = string.Empty;

        // Querystring takes precedence over the tags parameter. The results need to match the url
        if (!string.IsNullOrWhiteSpace(q1))
            newTags = q1;
        else if (!string.IsNullOrWhiteSpace(q2))
            newTags = string.Join(',', tags ?? Array.Empty<string>());

        var command = new GetRelatedTagsByTagsQuery(newTags, count: count);

        var results = await _mediator.Send(command);

        var model = new TagsModel(results, title, tags);

        return View(model);
    }
}