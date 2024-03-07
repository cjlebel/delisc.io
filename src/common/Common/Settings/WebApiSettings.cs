namespace Deliscio.Common.Settings;

public class WebApiSettings
{
    /// <summary>
    /// The name of the section in the config file that contains the MongoDb settings.
    /// </summary>
    public static string SectionName => "WebApiSettings";

    public string ApiKey { get; init; } = string.Empty;

    public string BaseUrl { get; init; } = string.Empty;
}