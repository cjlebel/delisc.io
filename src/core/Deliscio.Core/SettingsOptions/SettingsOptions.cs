using Deliscio.Core.Interfaces;

namespace Deliscio.Core.SettingsOptions;

public abstract class SettingsOptions : ISettingsOptions
{
    public virtual string SectionName { get; }
}