using Deliscio.Core.Interfaces;

namespace Deliscio.Core.SettingsOptions;

public abstract class SettingsOptions : ISettingsOptions
{
    public string SectionName { get; }
}