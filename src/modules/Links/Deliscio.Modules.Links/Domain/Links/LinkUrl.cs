using Ardalis.GuardClauses;
using Habanerio.Core.BuildingBlocks.Domain;

namespace Deliscio.Modules.Links.Domain.Links;
public sealed class LinkUrl : ValueObject
{
    public string Value { get; }

    public LinkUrl(string url)
    {
        var newUrl = url?.Trim();

        Guard.Against.NullOrEmpty(newUrl, nameof(url));

        Value = newUrl;
    }
}
