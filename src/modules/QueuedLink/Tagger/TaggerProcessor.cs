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

    private readonly Dictionary<string, string[]> _tagSuggestions = new()
    {
        // Tech
        { "agile", new [] { "agile", "scrum", "project management" } },
        { "apis", new [] { "api", "web services", "integration" } },
        { "artificial intelligence", new [] { "artificial intelligence", "AI", "machine learning", "deep learning" }},
        { "azure", new [] { "azure", "cloud", "microsoft" } },
        { "best practices", new[] { "best practices", "development", "coding" } },
        { "blockchain", new [] { "blockchain", "cryptocurrency", "smart contracts" }},
        { "clean architecture", new [] { "clean architecture", "software development", "software architecture" } },
        { "cloud", new [] { "cloud computing", "scalability", "aws", "azure" } },
        { "c#", new [] { "csharp", "c#" } },
        { "containerization", new[] { "docker", "kubernates", "k8" } },
        { "cybersecurity", new [] { "cybersecurity", "network security", "ethical hacking" }},
        { "data science", new [] { "data science", "machine learning", "data analysis", "big data" }},
        { "design patterns", new [] { "design patterns", "architecture", "best practices" } },
        { "development", new [] { "technology", "software", "development" } },
        { "devops", new [] { "devops", "automation", "cloud" } },
        { "docker", new [] { "docker", "containerization", "deployment" } },
        { "dotnet", new [] { ".net", "csharp", "microsoft", "dotnet" } },
        { "dotnet core", new [] { ".net core", "csharp", "microsoft", "dotnet" } },
        { "dotnet framework", new [] { ".net framework", "csharp", "microsoft", "dotnet" } },
        {"dotnetcurry", new [] {"dotnet"}},
        { "entity framework", new [] { "entity framework", "csharp", "microsoft", "dotnet" } },
        { "git", new [] { "git", "github", "version control" } },
        { "github", new []{"github"}},
        {"Go", new [] { "Go", "Go programming", "backend development" }},
        { "internet of things", new [] { "internet of things", "IoT", "connected devices" }},
        {"Java", new [] { "Java", "Java programming", "object-oriented" }},
        { "javascript", new [] { "javascript", "web", "frontend" } },
        {"Kotlin", new [] { "Kotlin", "Android development", "mobile apps" }},
        {"linux", new [] { "linux"}},
        {"macos", new [] { "macos", "apple"}},
        {"milan jovanovic", new [] { "milan jovanovic", ".net", "dotnet", "software development"}},
        {"milan jovanović", new [] { "milan Jovanovic", ".net", "dotnet", "software development"}},
        {"milanjovano", new [] { "milan Jovanovic", ".net", "dotnet", "software development"}},
        { "mobile apps", new [] { "mobile apps", "app development", "iOS", "Android" }},
        { "next.js", new [] { "next.js", "react", "frontend" } },
        {"nick chapsas", new [] {"nick chapsas", ".net", "dotnet", "software development"}},
        {"PHP", new [] { "PHP", "PHP programming", "server-side scripting" }},
        { "programming", new [] { "programming", "coding", "software development" }},
        {"Python", new [] { "Python", "Python programming", "scripting" }},
        { "react", new [] { "react", "javascript", "frontend" } },
        {"Ruby", new [] { "Ruby", "Ruby programming", "web development" }},
        {"Rust", new [] { "Rust", "systems programming", "high performance" }},
        { "security", new [] { "cybersecurity", "security", "hacking" } },
        { "software", new [] { "technology", "software", "development" } },
        { "stackoverflow", new[] { "stackoverflow" }},
        {"Swift", new [] { "Swift", "iOS development", "mobile apps" }},
        {"suse", new [] { "suse", "linux"}},
        { "technology", new [] { "technology", "software", "development" } },
        {"TypeScript", new [] { "TypeScript", "JavaScript", "static typing" }},
        {"ubuntu", new [] { "ubuntu", "linux"}},
        { "visual studio", new [] {"visual studio"}},
        { "visual studio code", new [] {"vscode"}},
        { "vs code", new [] {"vscode"}},
        {"windows", new [] { "windows", "microsoft"}},
        { ".net", new [] { ".net", "csharp", "microsoft", "dotnet" } },
        { "UI/UX design", new [] { "UI/UX design", "user interface", "user experience" }},
        { "virtual reality", new [] { "virtual reality", "VR", "immersive experiences" }},
        { "web development", new [] { "web development", "frontend", "backend", "full-stack" }},

       // Tools & Platforms
       { "WordPress", new [] { "WordPress", "blogging", "content management" }},
       { "Shopify", new [] { "Shopify", "e-commerce", "online store" }},
       { "Magento", new [] { "Magento", "e-commerce", "online store" }},
       { "Jira", new [] { "Jira", "project management", "issue tracking" }},
       { "Slack", new [] { "Slack", "team collaboration", "messaging" }},
       { "Trello", new [] { "Trello", "project management", "task organization" }},
       { "Google Cloud", new [] { "Google Cloud", "cloud computing", "GCP" }},
       { "AWS", new [] { "AWS", "Amazon Web Services", "cloud computing" }},
       { "Heroku", new [] { "Heroku", "cloud platform", "PaaS" }},
       { "Microsoft Azure", new [] { "Microsoft Azure", "cloud computing", "Azure services" }},

        // News
        {"cbc", new [] { "news", "cbc", "canada"}},
        {"cnbc", new [] {"news", "usa", "cnbc"}},
        {"cnn", new [] {"news", "usa", "cnn"}},
        {"ctv", new [] { "news", "ctv", "canada" }},
        {"globalnews", new [] { "news", "globalnews", "canada", "toronto"}},
        {"nationalpost", new [] { "news", "nationalpost", "canada", "toronto"}},
        {"thestar", new [] { "news", "thestar", "canada", "toronto"}},
        {"torontosun", new [] { "news", "torontosun", "canada", "toronto"}},


        // Foods & Recipes
        { "allrecipes", new [] { "recipes", "allrecipes"}},
        {"barbecue", new [] {"bbq", "food"}},
        {"barbeque", new [] {"bbq", "food"}},
        {"bbq", new []{"bbq", "food"}},
        { "bonappetit", new [] { "recipes", "bonappetit"}},
        { "cooking", new [] { "recipes", "cooking"}},
        { "cookinglight", new [] { "recipes", "cookinglight"}},
        { "delish", new [] { "recipes", "delish"}},
        { "epicurious", new [] { "recipes", "epicurious"}},
        { "foodnetwork", new [] { "recipes", "foodnetwork"}},
        { "one pot", new []{"one pot", "recipes"}},
        { "one-pot", new []{"one pot", "recipes"}},
        { "recipe", new [] {"recipes"}},
        { "recipes", new [] {"recipes"}},
        { "yummly", new [] { "recipes", "yummly"}},

        // Health & Fitness
        {"kettlebells", new [] { "kettlebells", "fitness"}},
        {"yoga", new [] { "yoga", "fitness"}},
        {"fitness", new [] { "fitness"}},
        {"health", new [] { "health"}},
        {"peloton", new [] { "cycling", "indoorcycling", "peloton"}},
        {"running", new [] { "running", "fitness"}},
        {"wahoo", new [] { "cycling", "indoorcycling", "wahoo"}},
        {"weightlifting", new [] { "weightlifting", "fitness"}},
        {"weights", new [] { "weights", "fitness"}},
        {"workout", new [] { "workout", "fitness"}},
        {"workouts", new [] { "workouts", "fitness"}},
        {"zwift", new [] { "cycling", "indoorcycling", "zwift"}},
        
        

        // Videos
        {"youtube", new []{"youtube", "video"}},

    };

    public TaggerProcessor(IMediator mediator, ILogger<TaggerProcessor> logger)
    {
        Guard.Against.Null(mediator);
        Guard.Against.Null(logger);

        _logger = logger;
        _mediator = mediator;
    }

    public async ValueTask<(bool IsSuccess, string Message, QueuedLink? Link)> ExecuteAsync(QueuedLink link, CancellationToken token = default)
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
                var titleTags = await Task.Run(() =>
                {
                    return _tagSuggestions.Where(x => title.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();
                }, token);

                var keywordTags = await Task.Run(() =>
                {
                    return _tagSuggestions.Where(x => title.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();
                }, token);

                var descriptionTags = await Task.Run(() =>
                {
                    return _tagSuggestions.Where(x => title.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();
                }, token);

                tags = (titleTags.Union(keywordTags).Concat(descriptionTags)).Distinct().ToArray();
            }
        }

        link = link with { Tags = tags.Distinct().ToArray() };
        link = link with { State = QueuedStates.TaggingCompleted };

        return (true, "Tagging Completed", link);
    }
}