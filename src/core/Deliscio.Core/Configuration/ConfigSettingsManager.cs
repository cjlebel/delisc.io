using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Deliscio.Core.Configuration;

/// <summary>
/// A Config helper class for strongly typed config settings files
/// </summary>
public static class ConfigSettingsManager
{
    private static IConfiguration _config;

    private static readonly IConfigurationSection _section;

    /// <summary>
    /// Due to SetBasePath - This property currently used mostly for tests and console type apps (not web apps and api)
    /// </summary>
    public static IConfiguration Configs => GetConfigs();

    public static string AspNetCoreEnvironmentVariableKey => "ASPNETCORE_ENVIRONMENT";

    public static string DotNetEnvironmentVariableKey => "DOTNET_ENVIRONMENT";

    public static string EnvironmentName
    {
        get
        {
            // Via AspNetCoreEnvironmentVariableKey
            var environmentName =
                Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariableKey, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrWhiteSpace(environmentName))
            {
                environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariableKey,
                    EnvironmentVariableTarget.User);

                if (string.IsNullOrWhiteSpace(environmentName))
                {
                    environmentName = Environment.GetEnvironmentVariable(AspNetCoreEnvironmentVariableKey,
                        EnvironmentVariableTarget.Process);
                }
            }

            // Via DotNetEnvironmentVariableKey (Worker Services don't recognize the AspNetCoreEnvironmentVariableKey)
            if (string.IsNullOrWhiteSpace(environmentName))
            {
                environmentName =
                    Environment.GetEnvironmentVariable(DotNetEnvironmentVariableKey, EnvironmentVariableTarget.Machine);

                if (string.IsNullOrWhiteSpace(environmentName))
                {
                    environmentName = Environment.GetEnvironmentVariable(DotNetEnvironmentVariableKey,
                        EnvironmentVariableTarget.User);

                    if (string.IsNullOrWhiteSpace(environmentName))
                    {
                        environmentName = Environment.GetEnvironmentVariable(DotNetEnvironmentVariableKey,
                            EnvironmentVariableTarget.Process);
                    }
                }

            }

            return environmentName;
        }
    }

    /// <summary>
    /// Gets the default appsettings.json, along with the environment specific one (if it exists), and merges them together.
    /// </summary>
    /// <returns>
    /// An IConfiguration object that contains the merged settings from the appsettings.json and appsettings.{environment}.json files
    /// </returns>
    public static IConfiguration GetConfigs()
    {
        var _config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", true, true)
            .AddJsonFile($"appsettings.{EnvironmentName}.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        return _config;
    }

    public static IConfigurationSection GetSection(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        return Configs.GetSection(name);
    }

    /// <summary>
    /// Gets the section banes on the nameof() the type.
    /// </summary>
    /// <typeparam name="T">The type to be retrieved and returned</typeparam>
    /// <returns>A settings object of type T</returns>
    public static T GetSection<T>() where T : IBaseConfigSettings
    {
        var nameOfT = typeof(T).Name;

        var rslt = Configs.GetSection(nameOfT).Get<T>();

        return rslt;
    }

    /// <summary>
    /// Gets the section with the specified name, but is of type T.
    /// This allows for multiple sections with different names, but of the same type to be used, instead of creating a class for each one
    /// See: Azure Storage settings
    /// </summary>
    /// <typeparam name="T">The type of the settings</typeparam>
    /// <param name="name">The name of the section of the settings.</param>
    /// <returns>A settings object of type T</returns>
    public static T GetSection<T>(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        return Configs.GetSection(name).Get<T>();
    }
}