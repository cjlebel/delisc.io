namespace Deliscio.Modules.UserProfiles.Common.Models.Requests;

public sealed record CreateUserProfileRequest
{
    public string UserId { get; init; }

    public string DisplayName { get; init; }

    public string Email { get; init; }

    public DateTimeOffset DateRegistered { get; init; }

    public CreateUserProfileRequest(string userId, string displayName, string email, DateTimeOffset dateRegistered)
    {
        UserId = userId;
        DisplayName = displayName;
        Email = email;
        DateRegistered = dateRegistered;
    }
}