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
}