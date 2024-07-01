namespace Deliscio.Modules.UserProfiles.Common.Models;

public sealed record UserProfile
{
    public string Id { get; set; }

    public string DisplayName { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; }

    public string? ImageUrl { get; set; }

    public string? Location { get; set; }

    public bool IsActivated { get; set; }

    public bool IsAdmin => Roles.Contains("Admin");

    public bool IsOnline { get; set; }

    public string[] Roles { get; set; } = Array.Empty<string>();

    public DateTimeOffset DateRegistered { get; set; }

    public string DateRegisteredFormatted => DateLastSeen.ToString("yyyy-MM-dd");

    public DateTimeOffset DateLastSeen { get; set; }

    public string DateLastSeenFormatted => DateLastSeen.ToString("yyyy-MM-dd");

    public UserProfile(string id, string email, string displayName, DateTimeOffset dateRegistered)
    {
        Id = id;
        Email = email;
        DisplayName = displayName;
        DateRegistered = dateRegistered;
    }

    public static UserProfile Create(string id, string email, string displayName, DateTimeOffset dateRegistered)
    {
        return new UserProfile(id, email, displayName, dateRegistered);
    }
}