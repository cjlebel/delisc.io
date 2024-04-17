using System.ComponentModel;
using Deliscio.Modules.Settings.Common;

namespace Deliscio.Common.Settings;

[SettingsProperties("Queued Links Settings", Description = "Settings for the Queued Links service.", GroupName= "Queued Links", Order = 2)]
public sealed record QueuedLinksSettingsV1 : ISettings
{
    public string Version { get { return "1"; } }

}

[SettingsProperties("Harvester Settings", Description = "Settings for the Queued Links Harvester service.", GroupName = "Queued Links", Order = 1)]
public sealed record QueuedLinksHarvesterSettingsV1 : ISettings
{
    public string Version { get { return "1"; } }

    public List<string> BannedHosts { get; set; } = new List<string>();
}