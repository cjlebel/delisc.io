namespace Deliscio.Modules.Settings.Common;

/// <summary>
/// Used to describe an actual settings class that is anywhere in the system.
/// </summary>
public class SettingsClassInfo
{
    public string Category => Categories is { Length: > 0 } ? Categories[0] : "";

    public string[]? Categories { get; set; } = [];

    public string Description { get; set; } = "";

    public string DisplayName { get; set; } = "";

    public string Name { get; set; } = "";

    public int Order { get; set; }

    public string Type { get; set; } = "";

    //public string Version { get; init; } = "1";

}