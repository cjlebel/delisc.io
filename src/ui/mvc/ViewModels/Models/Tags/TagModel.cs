namespace Deliscio.Web.Mvc.ViewModels.Models.Tags;

/// <summary>
/// Represents an individual tag pill that is displayed to the user
/// </summary>
/// <param name="Id">The id of the tag - use its index from the collection of tags</param>
/// <param name="Name">The name of the tag - the value that is displayed</param>
/// <param name="Href">The href of the tag - the value that is used for the link</param>
/// <param name="Count">The number of times this tag has been used</param>
/// <param name="Weight">
///     The weight of this tag compared to all other tags within a collection.
///     This is based on it's <see cref="Count"/> compared to the total count of all tags
/// </param>
public sealed record TagModel(int Id, string Name, string Href, int Count, float? Weight = 0f);