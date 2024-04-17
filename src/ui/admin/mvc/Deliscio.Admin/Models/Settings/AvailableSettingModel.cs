namespace Deliscio.Admin.Models.Settings;

public record AvailableSettingModel
{
    public string[] Category { get; set; } = [];

    public string Description { get; set; } = "";

    public string Name { get; set; } = "";

    public int Order { get; set; }

    public string Type { get; set; } = "";

    public string Version { get; init; } = "1";
}