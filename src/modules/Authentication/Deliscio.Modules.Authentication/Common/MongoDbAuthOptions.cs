namespace Deliscio.Modules.Authentication.Common;

public record MongoDbAuthOptions
{
    /// <summary>
    /// The name of the section in the config file that contains the MongoDb settings.
    /// </summary>
    public static string SectionName => "MongoDbAuthOptions";

    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    /// <value>
    /// The connection string.
    /// </value>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the database.
    /// </summary>
    /// <value>
    /// The name of the database.
    /// </value>
    public string DatabaseName { get; set; } = string.Empty;

    public string LoginPath { get; set; } = "/account/login";
}