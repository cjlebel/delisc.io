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

    private const string HARVESTER_ERROR_HTTPCLIENT = "{time}: Harvesting Error for: {url}\nMessage: {message}";

    public HarvesterProcessor(IMediator mediator, HttpClient httpClient, ILogger<HarvesterProcessor> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _mediator = mediator;
    }

    public async ValueTask<(bool IsSuccess, string Message, QueuedLink Link)> ExecuteAsync(QueuedLink link,
        CancellationToken token = default)
    {
        var updatedLink = link with { State = QueuedStates.FetchingMeta };

        var metaData = await Fetch(link.Url, token);

        if (!metaData.IsSuccess)
        {
            updatedLink = updatedLink with { State = QueuedStates.Error };
            return (false, $"Could not fetch the URL's page:\nError: {metaData.Message}", updatedLink);
        }

        //updatedLink = link with { MetaData = metaData.Result, State = QueuedStates.FetchingMetaCompleted };

        return (true, "Harvesting Completed", updatedLink);
    }

    /// <summary>
    /// Fetches the link's meta data, and returns a HarvestedLink object.
    /// </summary>
    /// <param name="url">The url of the page to go to</param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async Task<(bool IsSuccess, string Message, MetaData Result)> Fetch(string url, CancellationToken token = default)
    {
        (bool, string, MetaData) result;

        var htmlDocument = new HtmlDocument();

        try
        {
            var response = await _httpClient.GetAsync(url, token);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(token);

            htmlDocument.LoadHtml(content);
        }
        catch (Exception e)
        {
            _logger.LogError(HARVESTER_ERROR_HTTPCLIENT, DateTimeOffset.Now, url, e.Message);
            return (false, "Could not harvest the link", new MetaData());
        }

        var meta = new MetaData
        {
            Author = htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='author']")?.Attributes["content"]?.Value,
            CanonicalUrl =
                htmlDocument.DocumentNode.SelectSingleNode("//link[@rel='canonical']")?.Attributes["href"]?.Value,
            Description =
                htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='description']")?.Attributes["content"]?.Value,
            Keywords =
                htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='keywords']")?.Attributes["content"]?.Value,
            LastUpdate =
                htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='last-modified']")?.Attributes["content"]
                    ?.Value,
            OgImage = htmlDocument.DocumentNode.SelectSingleNode("//meta[@property='og:image']")?.Attributes["content"]
                    ?.Value,
            OgTitle =
                htmlDocument.DocumentNode.SelectSingleNode("//meta[@property='og:title']")?.Attributes["content"]
                    ?.Value,
            OgDescription =
                htmlDocument.DocumentNode.SelectSingleNode("//meta[@property='og:description']")?.Attributes["content"]
                    ?.Value,
            Title = htmlDocument.DocumentNode.SelectSingleNode("//title")?.InnerText,
        };

        result = (true, "Harvesting Complete", meta);

        return result;
    }
}