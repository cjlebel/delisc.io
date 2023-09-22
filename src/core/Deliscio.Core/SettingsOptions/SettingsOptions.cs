namespace Deliscio.Core.SettingsOptions;

public abstract class SettingsOptions
{
    public string SectionName { get; }

    protected SettingsOptions(string sectionName)
    {
        SectionName = sectionName;
    }
}