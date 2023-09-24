namespace Deliscio.Modules.QueuedLinks.Common.Models;

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