using Habanerio.Core.BuildingBlocks.Domain;

namespace Deliscio.Modules.Links.Domain.Links;
public sealed class LinkImageUrl : ValueObject
{
    public string Value { get; }

    public LinkImageUrl(string? value)
    {
        Value = value?.Trim() ?? string.Empty;
    }
}
