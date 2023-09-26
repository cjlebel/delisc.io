using Ardalis.GuardClauses;
using Deliscio.Modules.QueuedLinks.Common.Enums;
using Deliscio.Modules.QueuedLinks.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Deliscio.Modules.QueuedLinks.Tagger;

public class TaggerProcessor : ITaggerProcessor
{
    private readonly ILogger<TaggerProcessor> _logger;
    private readonly IMediator _mediator;

    private const string TAGGER_STARTED_MESSAGE = "{time}: Tagging Started for: {url}";
    private const string TAGGER_COMPLETED_MESSAGE = "{time}: Tagging Completed for: {url}";
    private const string TAGGER_ERROR_MESSAGE = "{time}: Tagging Error for: {url}\nMessage: {message}";

    private readonly Dictionary<string, string[]> _tagSuggestions = new Dictionary<string, string[]>
    {
        { "agile", new [] { "agile", "scrum", "project management" } },
        { "apis", new [] { "api", "web services", "integration" } },
        { "azure", new [] { "azure", "cloud", "microsoft" } },
        { "best practices", new[] { "best practices", "development", "coding" } },
        { "cloud", new [] { "cloud computing", "scalability", "aws", "azure" } },
        { "c#", new [] { "csharp", "c#" } },
        { "containerization", new[] { "docker", "kubernates", "k8" } },
        { "design patterns", new [] { "design patterns", "architecture", "best practices" } },
        { "development", new [] { "technology", "software", "development" } },
        { "devops", new [] { "devops", "automation", "cloud" } },
        { "docker", new [] { "docker", "containerization", "deployment" } },
        { "github", new []{"github"}},
        { "javascript", new [] { "javascript", "web", "frontend" } },
        { "next.js", new [] { "next.js", "react", "frontend" } },
        { "react", new [] { "react", "javascript", "frontend" } },
        { "security", new [] { "cybersecurity", "security", "hacking" } },
        { "software", new [] { "technology", "software", "development" } },
        { "stackoverflow", new[] { "stackoverflow" }},
        { "technology", new [] { "technology", "software", "development" } },
        { "visual studio", new [] {"visual studio"}},
        { ".net", new [] { ".net", "csharp", "microsoft", ".net", "dotnet" } },
        { "recipes", new [] {"recipes"}},
        {"nick chapsas", new [] {"nick chapsas", ".net", "dotnet", "software development"}},
        {"milan Jovanovic", new [] { "milan jovanovic", ".net", "dotnet", "software development"}},
        {"milan Jovanović", new [] { "milan Jovanovic", ".net", "dotnet", "software development"}},
        {"linux", new [] { "linux"}},
        {"ubuntu", new [] { "ubuntu", "linux"}},
        {"allrecipes", new [] { "recipes", "allrecipes"}},
        {"yummly", new [] { "recipes", "yummly"}},
        {"nationalpost", new [] { "news", "nationalpost"}},
        {"torontosun", new [] { "news", "torontosun"}},
        {"zwift", new [] { "cycling", "indoorcycling", "zwift"}}
    };

    public TaggerProcessor(IMediator mediator, ILogger<TaggerProcessor> logger)
    {
        Guard.Against.Null(mediator);
        Guard.Against.Null(logger);

        _logger = logger;
        _mediator = mediator;
    }

    public async ValueTask<(bool IsSuccess, string Message, QueuedLink Link)> ExecuteAsync(QueuedLink link, CancellationToken token = default)
    {
        _logger.LogInformation(TAGGER_STARTED_MESSAGE, DateTimeOffset.Now, link.Url);

        Guard.Against.Null(link);
        Guard.Against.NullOrWhiteSpace(link.Url);

        link = link with { State = QueuedStates.Tagging };

        string[] tags = Array.Empty<string>();
        if (link.MetaData != null)
        {
            var title = $"{link.MetaData.Title} {link.MetaData.OgTitle}";

            if (!string.IsNullOrWhiteSpace(title))
            {
                // This isn't proper ... this is to keep the ExecuteAsync contract.
                // Eventually this will call some other service/process to get the tags
                tags = await Task.Run(() =>
                {
                    return _tagSuggestions.Where(x => title.ToLower().Contains(x.Key)).SelectMany(x => x.Value).ToArray();
                }, token);
            }
        }

        link = link with { Tags = tags.Distinct().ToArray() };
        link = link with { State = QueuedStates.TaggingCompleted };

        return (true, "Tagging Completed", link);
    }
}