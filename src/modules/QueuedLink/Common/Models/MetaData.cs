namespace Deliscio.Modules.QueuedLinks.Common.Models;

public record MetaData
{
    public string Author { get; set; } = string.Empty;
    public string CanonicalUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string? LastUpdate { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
    public string OgDescription { get; set; } = string.Empty;
    public string OgTitle { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}