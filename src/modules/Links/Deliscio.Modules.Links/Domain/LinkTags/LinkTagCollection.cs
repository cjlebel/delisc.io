using System.Collections;
using System.Collections.Immutable;
using Habanerio.Core.BuildingBlocks.Domain;

namespace Deliscio.Modules.Links.Domain.LinkTags;

/// <summary>
/// Represents a collection of tags that can be associated with a link.
/// </summary>
public class LinkTagCollection : ValueObject, IEnumerable<LinkTag>
{
    private readonly List<LinkTag> _tags;

    public IReadOnlyCollection<LinkTag> Tags => _tags.ToImmutableList();

    private LinkTagCollection()
    {
        _tags = new List<LinkTag>();
    }

    public LinkTagCollection(IEnumerable<LinkTag> tags)
    {
        _tags = tags?.OrderBy(t => t.Name).ToList() ?? [];
        RecalculateWeights();
    }

    /// <summary>
    /// Creates an empty collection of tags
    /// </summary>
    /// <returns></returns>
    public static LinkTagCollection Empty()
    {
        return new LinkTagCollection();
    }

    /// <summary>
    /// Adds a single tag to the existing collection if it doesn't already exist.
    /// If it does exist, then the existing tag's count is incremented by 1
    /// And the weights of all tags are recalculated.
    /// </summary>
    /// <param name="tag"></param>
    /// <remarks>
    /// This calculates the weights of all tags after a tag has been added.
    /// Do not use this if enumerating over a collection of tags
    /// </remarks>
    public void Add(LinkTag tag)
    {
        if (tag is null)
            return;

        AddTag(tag);
        RecalculateWeights();
    }

    /// <summary>
    /// Adds one or more tag to the existing collection if they don't already exist.
    /// If any of them do exist, then the existing tags' count is incremented by 1
    /// And the weights of all tags are recalculated.
    /// </summary>
    public void Add(IEnumerable<LinkTag> tags)
    {
        if(tags is null || !tags.Any())
            return;

        foreach (var tag in tags)
        {
            AddTag(tag);
        }

        RecalculateWeights();
    }

    /// <summary>
    /// Permanently deletes a tag from the collection, regardless of how many times it's currently being used.
    /// </summary>
    /// <param name="tag"></param>
    public void Delete(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return;

        tag = tag.ToLowerInvariant();

        var tagToDelete = _tags
            .Find(t => t.Name.Equals(tag, StringComparison.InvariantCultureIgnoreCase));

        if (tagToDelete is null)
            return;

        //var tagToDeleteIndex = _tags.IndexOf(tagToDelete);
        _tags.Remove(tagToDelete);

        RecalculateWeights();
    }

    public void Delete(LinkTag tag)
    {
        Delete(tag?.Name);
    }

    /*
     * Not sure if I want these or not
     * Maybe rename, ReduceOrRemove?
     */
    //// <summary>
    //// If this tag exists then its count is decreased by one.
    //// If the count ends up being 0 after, then it is removed from the collection.
    //// All the tags' weights are recalculated.
    //// </summary>
    //// <param name="tag"></param>
    //public void Remove(LinkTag tag)
    //{
    //    RemoveTag(tag);
    //    RecalculateWeights();
    //}

    //public void Remove(LinkTag[] tags)
    //{
    //    if(tags is null || !tags.Any())
    //        return;

    //    foreach (var tag in tags)
    //    {
    //        RemoveTag(tag);
    //    }

    //    RecalculateWeights();
    //}


    /// <summary>
    /// Clears the collection of all tags
    /// </summary>
    public void Clear()
    {
        _tags.Clear();
    }

    public bool IsEmpty() => !_tags?.Any() ?? false;

    ///// <summary>
    ///// Gets the total Count for ALL tags within this collection
    ///// </summary>
    ///// <returns></returns>
    //public int Count() => _tags.Any() ? _tags.Count : 0;

    public IEnumerator<LinkTag> GetEnumerator()
    {
       return _tags.GetEnumerator();
    }

    public override string ToString()
    {
        return !_tags.Any() ? string.Empty : string.Join(", ", _tags.Select(t => t.Name).ToArray());
    }

    private void AddTag(LinkTag tag)
    {
        var tagToAdd = _tags.Find(t => t.Name.Equals(tag.Name, StringComparison.InvariantCultureIgnoreCase));

        // If this tag doesn't exist after initialization, then its has a count of 1
        if (tagToAdd is null)
        {
            _tags.Add(LinkTag.New(tag.Name.ToLowerInvariant()));
        }
        //else
        //{
        //    tagToAdd.IncreaseCount();
        //}
    }

    /// <summary>
    /// Removes a single occurence of it from the collection by reducing its count by 1.
    /// If count then equals 0, it is removed from the collection
    /// </summary>
    /// <param name="tag"></param>
    //private void RemoveTag(LinkTag tag)
    //{
    //    if(tag is null)
    //        return;

    //    var tagToRemove = _tags.Find(t => t.Name.Equals(tag.Name, StringComparison.InvariantCultureIgnoreCase));
    //    if (tagToRemove is null)
    //        return;

    //    tagToRemove.DecreaseCount();

    //    if (tagToRemove.Count <= 0)
    //        _tags.Remove(tagToRemove);
    //}

    private void RecalculateWeights()
    {
        var totalCount = _tags.Sum(t=>t.Count);

        foreach (var tag in _tags)
        {
            tag.CalculateWeight(totalCount);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}