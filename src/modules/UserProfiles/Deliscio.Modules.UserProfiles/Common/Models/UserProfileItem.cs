namespace Deliscio.Modules.UserProfiles.Common.Models;

public sealed class UserProfileItem
{
    public string Id { get; set; }

    public string DisplayName { get; set; }

    public bool IsActivated { get; set; }

    public bool IsAdmin => Roles.Contains("Admin");

    public bool IsOnline { get; set; }

    public string[] Roles { get; set; } = Array.Empty<string>();

    public DateTimeOffset DateRegistered { get; set; }

    public string DateRegisteredFormatted => DateLastSeen.ToString("yyyy-MM-dd");

    public DateTimeOffset DateLastSeen { get; set; }

    public string DateLastSeenFormatted => DateLastSeen.ToString("yyyy-MM-dd");
}