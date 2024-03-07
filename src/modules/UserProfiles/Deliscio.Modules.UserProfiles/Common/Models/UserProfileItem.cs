namespace Deliscio.Modules.UserProfiles.Common.Models;

public sealed record UserProfileItem
{
    public string Id { get; set; }

    public string DisplayName { get; set; }

    public bool IsActivated { get; set; }

    public bool IsOnline { get; set; }

    public string[] Roles { get; set; } = Array.Empty<string>();

    public DateTime DateLastSeen { get; set; }

    public string DateLastSeenFormatted => DateLastSeen.ToString("yyyy-MM-dd");
}