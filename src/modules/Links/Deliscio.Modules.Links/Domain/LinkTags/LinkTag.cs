using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Infrastructure.Data.Entities;
using Habanerio.Core.BuildingBlocks.Domain;

namespace Deliscio.Modules.Links.Domain.LinkTags;

/// <summary>
/// Represents a tag that is associated with a link.
/// </summary>
public sealed class LinkTag : ValueObject
{
    public string Name { get; }

    /// <summary>
    /// The number of times users have associated this tag has been used with a specific link.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// The weight of the tag, based on the total count of all tags for a link
    /// </summary>
    public decimal Weight { get; set; }

    private LinkTag(string name, int count, decimal weight)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Link tag name cannot be empty", nameof(name));
        }

        Name = name.Trim().ToLowerInvariant();
        Count = count;
        Weight = weight;
    }

    public static LinkTag New(string name)
    {
        return new LinkTag(name, 1, 0);
    }

    public static LinkTag Create(string name, int count, decimal weight)
    {
        return new LinkTag(name, count, weight);
    }

    public static LinkTag Map(LinkTagEntity entity)
    {
        if (entity is null || string.IsNullOrWhiteSpace(entity.Name))
        {
            return null;
        }

        return new LinkTag(entity.Name, entity.Count, entity.Weight);
    }

    public void IncreaseCount()
    {
        Count += 1;
    }

    public void DecreaseCount()
    {
        if (Count - 1 < 0)
            return;

        Count -= 1;
    }

    public void SetCount(int count)
    {
        Guard.Against.Negative(count, nameof(count));

        Count = count;
    }

    /// <summary>
    /// Calculates the weight of the tag based on the total count of from ALL tags.
    /// </summary>
    /// <param name="totalTagsCount">The total count across all tags</param>
    public void CalculateWeight(int totalTagsCount)
    {
        if (totalTagsCount > 0)
            Weight = decimal.Round((1m * Count/ totalTagsCount), 3);
    }
}
