using Microsoft.Extensions.Configuration;

namespace Deliscio.Core.Configuration;

public interface IBaseConfigSettings
{
    /// <summary>
    /// Gets or sets the name of the appsettings section for this set of settings.
    /// </summary>
    /// <value>
    /// The name of the section.
    /// </value>
    string SectionName { get; set; }

    IConfigurationSection Settings { get; set; }

    /// <summary>
    /// Gets the specific app setting as a string.
    /// </summary>
    /// <param name="key">The name of the key to retrieve the setting's value from</param>
    /// <returns>The value of the settings as a string</returns>
    string GetAppSetting(string key);

    /// <summary>
    /// Gets the specific app setting as an int.
    /// </summary>
    /// <param name="key">The name of the key to retrieve the setting's value from</param>
    /// <returns>The value of the settings as an int</returns>
    int GetIntAppSetting(string key);

    /// <summary>
    /// Gets the specific app setting as an or a default value.
    /// </summary>
    /// <param name="key">The name of the key to retrieve the setting's value from</param>
    /// <param name="defaultValue">If the key cannot be found, then use this value instead</param>
    /// <returns>The value of the settings as an int</returns>
    int GetIntOrDefaultAppSetting(string key, int defaultValue);

    /// <summary>
    /// Gets the specific app setting as a decimal.
    /// </summary>
    /// <param name="key">The name of the key to retrieve the setting's value from</param>
    /// <returns>The value of the settings as a decimal</returns>
    decimal GetDecimalAppSetting(string key);

    /// <summary>
    /// Gets the specific app setting as an or a default value.
    /// </summary>
    /// <param name="key">The name of the key to retrieve the setting's value from</param>
    /// <param name="defaultValue">If the key cannot be found, then use this value instead</param>
    /// <returns>The value of the settings as an int</returns>
    decimal GetDecimalOrDefaultAppSetting(string key, decimal defaultValue);
}

public abstract class BaseConfigSettings : IBaseConfigSettings
{
    protected BaseConfigSettings()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseAppSettings"/> class.
    /// Note: Haven't tested these yet
    /// </summary>
    /// <param name="name">The name.</param>
    protected BaseConfigSettings(string name)
    {
        Settings = ConfigSettingsManager.Configs.GetSection(name);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseAppSettings"/> class.
    /// Note: Haven't tested these yet
    /// </summary>
    /// <param name="section">The section of the app settings to use.</param>
    protected BaseConfigSettings(IConfigurationSection section)
    {
        Settings = section;
    }

    public string SectionName { get; set; }

    public IConfigurationSection Settings { get; set; }

    //public abstract BaseAppSettings Get();

    /// <summary>
    /// Gets the section of type T from the settings file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static T Get<T>(string name)
    {
        return ConfigSettingsManager.GetSection<T>(name);
    }

    #region - Helper Methods

    public string GetAppSetting(string key)
    {
        var s = Settings[key];
        return string.IsNullOrWhiteSpace(s) ? string.Empty : s;
    }

    public int GetIntAppSetting(string key)
    {
        var s = Settings[key];
        return int.TryParse(s, out var result) ? result : 0;
    }

    public int GetIntOrDefaultAppSetting(string key, int defaultValue)
    {
        var s = Settings[key];
        return int.TryParse(s, out var result) ? result : defaultValue;
    }

    public decimal GetDecimalAppSetting(string key)
    {
        var s = Settings[key];
        return !string.IsNullOrWhiteSpace(s) ? decimal.Parse(s) : 0;
    }

    public decimal GetDecimalOrDefaultAppSetting(string key, decimal defaultValue)
    {
        var s = Settings[key];
        return decimal.TryParse(s, out var result) ? result : defaultValue;
    }

    #endregion
}