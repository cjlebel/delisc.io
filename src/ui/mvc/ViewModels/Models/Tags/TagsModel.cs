using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Web.Mvc.ViewModels.Models.Tags;

/// <summary>
/// Represents a collection of individual tag pills that are displayed to the user
/// </summary>
public sealed class TagsModel
{
    public IEnumerable<TagModel> Tags { get; } = Enumerable.Empty<TagModel>();

    /// <summary>
    /// The title of the tag collection
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TagsModel"/> class.
    /// </summary>
    /// <param name="tags">The collection of tags to create links from</param>
    /// <param name="title">The title of the collection (eg: Related Tags)</param>
    /// <param name="relatedTags">A collection of tags that were used to get the tags[]. This is used to create the url for each tag in tags</param>
    public TagsModel(IEnumerable<LinkTag>? tags, string title = "", string[]? relatedTags = default)
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

                // Initialize a new array with the current tag and the related tags
                var allTags = relatedTags == null ? new string[1] : new string[relatedTags.Length + 1];
                relatedTags?.CopyTo(allTags, 0);

                allTags[^1] = tag.Name.ToLowerInvariant();

                var tagQuery = string.Join(",", allTags.OrderBy(t => t).ToArray());

                var href = $"/links?t={tagQuery.Replace(" ", "+")}";

                pills.Add(new TagModel(idx + 1, tag.Name, href, tag.Count, weight));
            }

            Tags = pills.AsEnumerable();
        }
    }
}