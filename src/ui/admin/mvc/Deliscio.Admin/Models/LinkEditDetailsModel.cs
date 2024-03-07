using Deliscio.Modules.Links.Common.Models;

namespace Deliscio.Admin.Models;

public sealed record LinkEditDetailsModel
{
    private LinkItem[] _relatedLinks = Array.Empty<LinkItem>();

    public Link? Link { get; init; }

    public string? ErrorMessage { get; init; }

    public bool IsSuccess { get; init; }

    /// <summary>
    /// RelatedLinks can receive a null (to limit the number of times we have to check for null) but it will never be null.
    /// </summary>
    public LinkItem[]? RelatedLinks
    {
        get
        {
            return _relatedLinks;
        }
        set
        {
            if (value is not null)
                _relatedLinks = value;
        }
    }

    /// <summary>
    /// Gets the Url of the page that brought us to this details page.
    /// Use case: User did a search and clicked the link for this. We want to preserve the search results.
    /// </summary>
    public string ReturnUrl { get; set; } = "";

    private LinkEditDetailsModel(Link? link, string errorMessage, bool isSuccess)
    {
        Link = link;
        ErrorMessage = errorMessage;
        IsSuccess = isSuccess;
    }


    public static LinkEditDetailsModel Success(Link link, string returnUrl = "", LinkItem[]? relatedItems = default)
    {
        var rslt = new LinkEditDetailsModel(link, string.Empty, true)
        {
            RelatedLinks = relatedItems ?? Array.Empty<LinkItem>(),
            ReturnUrl = returnUrl
        };

        return rslt;
    }

    public static LinkEditDetailsModel Failure(string errorMessage)
    {
        return new LinkEditDetailsModel(null, errorMessage, false);
    }
}