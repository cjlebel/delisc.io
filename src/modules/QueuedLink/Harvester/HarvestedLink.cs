using Deliscio.Modules.QueuedLinks.Common.Models;

namespace Deliscio.Modules.QueuedLinks.Harvester;

/// <summary>
/// Extension of the QueuedLink model to include the harvested meta data
/// </summary>
public record HarvestedLink : QueuedLink
{
    public MetaData Meta { get; set; }
}

public record MetaData
{
    public string? Author { get; set; }
    public string? CanonicalUrl { get; set; }
    public string? Description { get; set; }
    public string? Keywords { get; set; }
    public string? LastUpdate { get; set; }
    public string? OgImage { get; set; }
    public string? OgDescription { get; set; }
    public string? OgTitle { get; set; }
    public string? Title { get; set; }
}