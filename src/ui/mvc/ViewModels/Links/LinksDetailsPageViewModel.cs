using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Web.Mvc.ViewModels.Links;

public record LinksDetailsPageViewModel : BasePageViewModel
{
    public string Id { get; set; }

    public string Description { get; set; }

    public string Domain { get; set; }

    public string ImageUrl { get; set; }

    public string Title { get; set; }

    public List<LinkTag> Tags { get; set; } = new();

    public string Url { get; set; }
}