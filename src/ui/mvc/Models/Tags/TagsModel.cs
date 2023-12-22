using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Web.Mvc.Models.Tags;

/// <summary>
/// Represents a collection of individual tag pills that are displayed to the user
/// </summary>
public sealed class TagsModel
{
    public IEnumerable<TagModel> Tags { get; } = Enumerable.Empty<TagModel>();

    public string Title { get; }

    public TagsModel(IEnumerable<LinkTag>? tags, string title = "")
    {
        Title = title;

        var linkTags = tags?.ToArray() ?? Array.Empty<LinkTag>();

        if (tags is not null && linkTags.Any())
        {
            var pills = new List<TagModel>();
            var totalCount = linkTags.Sum(x => x.Count);

            for (var idx = 0; idx < linkTags.Length; idx++)
            {
                var tag = linkTags[idx];
                var weight = totalCount > 0 ? (float)tag.Count / totalCount : 0f;
                var href = $"/tags/{tag.Name.Replace(" ", "+")}";

                pills.Add(new TagModel(idx+1, tag.Name, href, tag.Count, weight));
            }

            Tags = pills.AsEnumerable();
        }
    }
}