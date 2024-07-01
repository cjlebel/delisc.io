using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;

namespace Deliscio.Web.Admin.TagHelpers.Pager;

public class PagerTagHelper : TagHelper
{
    public int Page { get; set; }

    public int TotalPages { get; set; }

    public int? TotalResults { get; set; }


    public string TargetPage { get; set; } = string.Empty;

    /// <summary>
    /// Collection of query string parameters to use when generating the pager links
    /// </summary>
    private Dictionary<string, string?> QueryParams { get; set; } = new();

    /// <summary>
    /// Used to get the current request (can't access HttpContext without it)
    /// </summary>
    [ViewContext] public ViewContext? ViewContext { get; set; } = null;

    /// <summary>
    /// The current HttpContext.Request
    /// </summary>
    private HttpRequest? HttpRequest => ViewContext?.HttpContext.Request ?? null;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (string.IsNullOrWhiteSpace(TargetPage))
        {
            TargetPage = HttpRequest?.Path ?? string.Empty;
        }

        if (HttpRequest?.Query is { Count: > 0 })
        {
            /* Ignore 'page' and 'size' */
            QueryParams = HttpRequest.Query?.Where(
                    kvp => !kvp.Key.Equals("page", StringComparison.CurrentCultureIgnoreCase) &&
                          !kvp.Key.Equals("size", StringComparison.CurrentCultureIgnoreCase))
                .Select(kvp => kvp).ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString()) ??
                new Dictionary<string, string?>();
        }

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var sb = new StringBuilder();
        sb.Append("<nav aria-label=\"Page navigation example\">");
        sb.Append("<ul class=\"pagination\">");

        AddFirstPage(sb);
        AddPreviousPage(sb);

        AddTotalResults(sb);

        AddNextPage(sb);
        AddLastPage(sb);

        output.Content.SetHtmlContent(sb.ToString());
    }

    private void AddFirstPage(StringBuilder sb)
    {
        if (Page > 1)
        {
            var qs = new Dictionary<string, string?>(QueryParams);

            var url = QueryHelpers.AddQueryString(TargetPage, qs);

            sb.Append($"<li class=\"page-item first\"><a class=\"page-link\" href=\"{url}\">First</a></li>");
        }
        else
        {
            sb.Append("<li class=\"page-item first disabled\" aria-disabled=\"true\"><a class=\"page-link\" href=\"/\">First</a></li>");
        }
    }

    private void AddPreviousPage(StringBuilder sb)
    {
        if (Page > 1)
        {
            var qs = new Dictionary<string, string?>(QueryParams);
            qs.Add("page", (Page - 1).ToString());

            var url = QueryHelpers.AddQueryString(TargetPage, qs);

            sb.Append($"<li class=\"page-item previous\"><a class=\"page-link\" href=\"{url}\">Previous</a></li>");
        }
        else
        {
            sb.Append($"<li class=\"page-item previous disabled\" aria-disabled=\"true\"><a class=\"page-link\" href=\"/\">Previous</a></li>");
        }
    }

    private void AddNextPage(StringBuilder sb)
    {
        if (Page < TotalPages)
        {
            var qs = new Dictionary<string, string?>(QueryParams);
            qs.Add("page", (Page + 1).ToString());

            var url = QueryHelpers.AddQueryString(TargetPage, qs);

            sb.Append($"<li class=\"page-item next\"><a class=\"page-link\" href=\"{url}\">Next</a></li>");
        }
        else
        {
            sb.Append("<li class=\"page-item next disabled\"><a class=\"page-link\" href=\"/\">Next</a></li>");
        }
    }

    private void AddLastPage(StringBuilder sb)
    {
        var qs = new Dictionary<string, string?>(QueryParams);
        qs.Add("page", (TotalPages).ToString());

        var url = QueryHelpers.AddQueryString(TargetPage, qs);

        if (Page < TotalPages)
        {
            sb.Append($"<li class=\"page-item last\"><a class=\"page-link\" href=\"{url}\">Last</a></li>");
        }
        else
        {
            sb.Append($"<li class=\"page-item last disabled\"><a class=\"page-link\" href=\"{url}\">Last</a></li>");
        }
    }

    private void AddTotalResults(StringBuilder sb)
    {
        if (TotalResults.HasValue)
        {
            sb.Append($"<li class=\"page-item total-results\"><span class=\"page-link\">{TotalResults} results</span></li>");
        }
    }
}