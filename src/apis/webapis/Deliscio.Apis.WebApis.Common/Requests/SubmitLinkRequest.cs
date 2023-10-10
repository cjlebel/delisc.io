using System.ComponentModel.DataAnnotations;

namespace Deliscio.Apis.WebApi.Common.Requests;

public sealed record SubmitLinkRequest
{
    /// <summary>
    /// The url of the link to submit
    /// </summary>
    [Required]
    public string Url { get; set; }

    /// <summary>
    /// The id of the user who is submitting the link
    /// </summary>
    [Required]
    public string SubmittedById { get; set; }

    #region - User Optional Properties -

    /// <summary>
    /// A set of tags for the link. These will be added/appended to the central link and added to the user's link
    /// </summary>
    public string[] Tags { get; set; } = Array.Empty<string>();

    /// <summary>
    /// The title for the link, that will only be used in the user's link
    /// </summary>
    public string Title { get; set; } = string.Empty;
    #endregion

    public SubmitLinkRequest() { }

    /// <summary>
    /// Represents a request by a user to submit a link
    /// </summary>
    /// <param name="url">The url of the link to be submitted</param>
    /// <param name="submittedById">The id of the user who is submitting the link</param>
    public SubmitLinkRequest(string url, string submittedById)
    {
        Url = url;
        SubmittedById = submittedById;
    }

    public (bool Value, List<ValidationResult> Errors) IsValid()
    {
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);

        return (isValid, validationResults);
    }
}