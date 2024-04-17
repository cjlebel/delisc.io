using System.Reflection;

namespace Deliscio.Modules.Settings.Common;

public class SettingsHelper
{
    public static IEnumerable<SettingsClassInfo> Discover(Type type)
    {
        var rslts = new List<SettingsClassInfo>();

        var types = Assembly.GetAssembly(type)?
                            .GetTypes()
                            .Where(t => typeof(ISettings).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false })?.ToList() ??
                        [];

        if (types is { Count: 0 })
            yield break;

        foreach(var t in types)
        {
            var category = t.GetCustomAttribute<SettingsPropertiesAttribute>()?.GroupName ?? "Miscellaneous";
            var description = t.GetCustomAttribute<SettingsPropertiesAttribute>()?.Description ?? string.Empty;
            var name = t.GetCustomAttribute<SettingsPropertiesAttribute>()?.Name ?? throw new KeyNotFoundException(nameof(SettingsPropertiesAttribute.DisplayName));
            var displayName = t.GetCustomAttribute<SettingsPropertiesAttribute>()?.DisplayName ?? name;
            var order = t.GetCustomAttribute<SettingsPropertiesAttribute>()?.Order ?? 999;

            yield return new SettingsClassInfo
            {
                Categories = new[] { category },
                Description = description,
                Name = name,
                Order = order,
                Type = t.Name
            };
        }
    }
}