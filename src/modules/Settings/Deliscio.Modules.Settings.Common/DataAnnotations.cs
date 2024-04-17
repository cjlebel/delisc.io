namespace Deliscio.Modules.Settings.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SettingsPropertiesAttribute(string name) : Attribute
{
    public string Name { get; set; } = name;

    /// <summary>
    /// Used to describe this set of settings
    /// Eg: Privacy Settings
    /// </summary>
    public string DisplayName { get; set; } = "";

    /// <summary>
    /// Used to describe this set of settings
    /// Eg: "Settings for managing the privacy of the website"
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// Used to group multiple settings classes together
    /// Eg: Website
    /// </summary>
    public string GroupName { get; set; } = "";

    /// <summary>
    /// Used to order the settings in the UI
    /// If it's part of a group, it will order the settings within the group
    /// </summary>
    public int Order { get; set; } = 1;

    /// <summary>
    /// The version of the settings
    /// </summary>
    public int Version { get; set; } = 1;
}


/*
    - Website Settings
        - Main
            - Site Name
            - Site Description
            - Links Per Page
        - Privacy
            - Allow Cookies
            - Allow Tracking
            - Allow Analytics
        - Looks
            - Theme
            - Font Size
            - Font Family
        - Queued Links
            - Harvester
                - Banned Hosts
            - Queue
                - Max Queue Size
                - Max Queue Age
  
  
  
*/