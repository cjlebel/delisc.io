using Ardalis.GuardClauses;
using Habanerio.Core.BuildingBlocks.Domain;

namespace Deliscio.Modules.Links.Domain.Links;
public sealed class LinkTitle : ValueObject
{
    public string Value { get; }

    public LinkTitle(string title)
    {
        var newTitle = title?.Trim();

        Guard.Against.NullOrEmpty(newTitle, nameof(title));

        Value = newTitle;
    }
}
