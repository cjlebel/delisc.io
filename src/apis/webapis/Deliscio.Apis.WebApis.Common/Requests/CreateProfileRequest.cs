namespace Deliscio.Apis.WebApi.Common.Requests;

public sealed record CreateProfileRequest
{
    public string Bio { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// The name that the user would like to be displayed as
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;



    /// <summary>
    /// A collection of social media names and the user's values for them
    /// </summary>
    public Dictionary<string, string> SocialLinks { get; set; } = new();

    /// <summary>
    /// AuthUser's personal website
    /// </summary>
    public string Website { get; set; } = string.Empty;
}