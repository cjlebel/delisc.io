using Deliscio.Modules.Settings.Common;
namespace Deliscio.Common.Settings;

[SettingsProperties("Main Website Settings", DisplayName = "Main", Description = "Settings for the website.", GroupName = "Website Settings", Order = 1)]
public sealed class WebsiteSettingsV1 : ISettings
{
    public string Version { get { return "1"; } }

    public string SiteName { get; init; } = string.Empty;

    public string SiteDescription { get; init; } = string.Empty;

    public int LinksPerPage { get; init; } = 25;
}