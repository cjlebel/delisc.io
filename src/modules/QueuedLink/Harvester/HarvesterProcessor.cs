using System.Net;
using Ardalis.GuardClauses;
using Deliscio.Modules.QueuedLinks.Common.Enums;
using Deliscio.Modules.QueuedLinks.Common.Models;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.QueuedLinks.Harvester;

public class HarvesterProcessor : IHarvesterProcessor
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HarvesterProcessor> _logger;
    private readonly IMediator _mediator;

    private const string HARVESTER_ERROR_HTTP_CLIENT = "{time}: Harvesting Error for: {url} {newLine}Message: {message}";

    public HarvesterProcessor(IMediator mediator, HttpClient httpClient, ILogger<HarvesterProcessor> logger)
    {
        Guard.Against.Null(mediator);
        Guard.Against.Null(httpClient);
        Guard.Against.Null(logger);

        _httpClient = httpClient;
        _logger = logger;
        _mediator = mediator;

        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.0.0 Safari/537.36");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async ValueTask<(bool IsSuccess, string Message, QueuedLink? Link)> ExecuteAsync(QueuedLink link, CancellationToken token = default)
    {
        link = link with { State = QueuedStates.FetchingData };

        var metaData = await Fetch(link.Url, token);

        if (!metaData.IsSuccess)
        {
            link = link with { State = QueuedStates.Error };
            return (false, $"Could not fetch the URL's page:\nError: {metaData.Message}", link);
        }

        link = link with { Title = metaData.Result.Title ?? "", Description = metaData.Result.Description ?? string.Empty, MetaData = metaData.Result, State = QueuedStates.FetchingDataCompleted };

        return (true, "Harvesting Completed", link);
    }

    /// <summary>
    /// Fetches the link's meta data, and returns a HarvestedLink object.
    /// </summary>
    /// <param name="url">The url of the page to go to</param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<(bool IsSuccess, string Message, MetaData Result)> Fetch(string url, CancellationToken token = default)
    {
        var htmlDocument = new HtmlDocument();
        HttpResponseMessage? response = null;

        try
        {
            response = await _httpClient.GetAsync(url, token);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(token);

            htmlDocument.LoadHtml(content);
        }
        catch (Exception e)
        {
            _logger.LogError(HARVESTER_ERROR_HTTP_CLIENT, DateTimeOffset.Now, url, e.Message, Environment.NewLine);
            var statusCode = response?.StatusCode ?? HttpStatusCode.BadRequest;

            return (false, $"Could not harvest the link\nStatus: {statusCode}", new MetaData());
        }

        var ogTitle = WebUtility.HtmlDecode(htmlDocument.DocumentNode.SelectSingleNode("//meta[@property='og:title']")
            ?.Attributes["content"]
            ?.Value ?? string.Empty);

        var title = WebUtility.HtmlDecode(htmlDocument.DocumentNode.SelectSingleNode("//title")?.InnerText ??
                                          string.Empty);

        if (string.IsNullOrWhiteSpace(title))
        {
            if (!string.IsNullOrWhiteSpace(ogTitle))
                title = ogTitle;
            else
                title = url;
        }

        var description =
            htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='description']")?.Attributes["content"]?.Value;

        var ogDescription =
            WebUtility.HtmlDecode(htmlDocument.DocumentNode.SelectSingleNode("//meta[@property='og:description']")
                ?.Attributes["content"]
                ?.Value ?? string.Empty);

        if (string.IsNullOrWhiteSpace(description))
        {
            if (!string.IsNullOrWhiteSpace(ogDescription))
                description = ogDescription;
            else
                description = string.Empty;
        }

        var meta = new MetaData
        {
            Title = title,
            OgTitle = ogTitle,
            Description = description,
            OgDescription = ogDescription,

            Author = htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='author']")?.Attributes["content"]?.Value ?? string.Empty,
            CanonicalUrl = htmlDocument.DocumentNode.SelectSingleNode("//link[@rel='canonical']")?.Attributes["href"]?.Value ?? string.Empty,
            Keywords = htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='keywords']")?.Attributes["content"]?.Value ?? string.Empty,
            LastUpdate = htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='last-modified']")?.Attributes["content"]?.Value ?? string.Empty,
            OgImage = htmlDocument.DocumentNode.SelectSingleNode("//meta[@property='og:image']")?.Attributes["content"]?.Value ?? string.Empty
        };

        var result = (true, "Harvesting Complete", meta);

        return result;
    }
}