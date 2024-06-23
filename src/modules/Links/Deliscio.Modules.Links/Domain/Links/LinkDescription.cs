using Habanerio.Core.BuildingBlocks.Domain;

namespace Deliscio.Modules.Links.Domain.Links;
public sealed class LinkDescription : ValueObject
{
    public string Value { get; }

    public LinkDescription(string? value)
    {
        Value = value?.Trim() ?? string.Empty;
    }
}
