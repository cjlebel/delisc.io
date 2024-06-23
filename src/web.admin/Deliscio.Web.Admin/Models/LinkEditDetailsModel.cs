using Deliscio.Modules.Links.Application.Dtos;

namespace Deliscio.Web.Admin.Models;

public sealed record LinkEditDetailsModel
{
    public LinkDto? Link { get; init; }

    public string? ErrorMessage { get; init; }

    public bool IsSuccess { get; init; }

    /// <summary>
    /// RelatedLinks can receive a null (to limit the number of times we have to check for null) but it will never be null.
    /// </summary>
    public RelatedLinkDto[] RelatedLinks { get; set; } = [];

    /// <summary>
    /// Gets the Url of the page that brought us to this details page.
    /// Use case: User did a search and clicked the link for this. We want to preserve the search results.
    /// </summary>
    public string ReturnUrl { get; set; } = "";

    private LinkEditDetailsModel(LinkDto? link, string errorMessage, bool isSuccess)
    {
        Link = link;
        ErrorMessage = errorMessage;
        IsSuccess = isSuccess;
    }


    public static LinkEditDetailsModel Ok(LinkDto link, string returnUrl = "", RelatedLinkDto[]? relatedItems = default)
    {
        var rslt = new LinkEditDetailsModel(link, string.Empty, true)
        {
            RelatedLinks = relatedItems ?? [],
            ReturnUrl = returnUrl
        };

        return rslt;
    }

    public static LinkEditDetailsModel Failed(string errorMessage)
    {
        return new LinkEditDetailsModel(null, errorMessage, false);
    }
}