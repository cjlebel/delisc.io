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
        {"amazon", new [] { "amazon"}},
        {"amichai", new [] { "amichai", "microsoft", "ddd", "youtube", "dotnet", "architecture" } },
        { "apis", new [] { "api", "web services", "integration" } },
        {"atspotify", new []{"atspotify", "spotify", "blog", "engineering"}},
        { "azure", new [] { "azure", "cloud", "microsoft" } },
        {"atlassian", new [] { "atlassian", "ci-cd", "cloud"}},
        {"aws", new [] { "aws", "amazon", "cloud"}},
        { "bdd", new[] { "bdd", "development", "software", "architecture" } },
        {"blazor", new []{"b;azor", "dotnet", "frontend"}},
        { "best practices", new[] { "best practices", "development", "coding" } },
        { "blockchain", new [] { "blockchain", "cryptocurrency", "smart contracts" }},
        { "bootstrap", new [] { "bootstrap", "ui", "frameworks" }},
        { "browserstack", new [] { "browserstack", "ui", "frontend", "testing" }},
        { "clean architecture", new [] { "clean architecture", "software development", "software architecture", "design patterns" } },
        { "cloud", new [] { "cloud computing", "scalability", "aws", "azure" } },
        { "c#", new [] { "csharp", "c#" } },
        { "containerization", new[] { "docker", "kubernates", "k8", "containers" } },
        { "cqrs", new[] { "cqrs", "software" } },
        {"creative tim", new[] { "creative tim", "frontend", "ui", "templates" } },
        {"creative-tim", new[] { "creative tim", "frontend", "ui", "templates" } },
        { "css", new [] { "css", "frontend" } },
        { "cybersecurity", new [] { "cybersecurity", "network security", "ethical hacking" }},
        { "dapr", new [] { "dapr", "orm", "database", "sql" }},
        { "data science", new [] { "data science", "machine learning", "data analysis", "big data" }},
        { "design patterns", new [] { "design patterns", "architecture", "best practices" } },
        { "development", new [] { "technology", "software", "development" } },
        { "devops", new [] { "devops", "gitops", "automation", "cloud" } },
        {"devto", new []{"devto"}},
        {"dev.to", new []{"devto"}},
        { "diagram", new [] { "diagrams", "diagramming" } },
        { "diagrams", new [] { "diagrams", "diagramming" } },
        { "docker", new [] { "docker", "containerization", "deployment" } },
        {"domain driven", new [] { "ddd", "architecture", "software" } },
        {"domain-driven", new [] { "ddd", "architecture", "software" } },
        { "dotnet", new [] { "csharp", "dotnet" } },
        { "dotnet core", new [] { ".net core", "csharp", "dotnet" } },
        { "dotnet framework", new [] { ".net framework", "csharp", "dotnet" } },
        {"dotnetcurry", new [] {"dotnet"}},
        { "draw.io", new [] { "draw.io", "diagrams", "diagramming" } },
        {"drone ci", new []{"drone ci", "ci-cd"}},
        { "elon musk", new [] { "elon", "tesla", "spacex" } },
        { "entity framework", new [] { "entity framework", "csharp", "orm", "sql", "dotnet" } },
        { "ef core", new [] { "entity framework", "csharp", "dotnet" } },
        { "freecodecamp", new [] { "freecodecamp", "coding" } },
        { "smashingmagazine", new [] { "smashingmagazine", "ui", "seo" } },
        {"gitops", new []{"gitops", "ci-cd", "devops"}},
        {"graphql", new [] { "graphql", "apis" }},
        {"hangfire", new []{"hangfire", "pubsub", "dotnet"}},
        {"hanselman", new []{"hanselman", "dotnet", "blog"}},
        { "hashicorp", new [] { "hashicorp", "ci-cd" }},
        { "internet of things", new [] { "internet of things", "IoT", "connected devices" }},
        {"Java", new [] { "Java", "Java programming", "object-oriented" }},
        { "javascript", new [] { "javascript", "web", "frontend" } },
        {"jest", new [] { "jest", "testing", "frontend", "ui" }},
        {"johnny harris", new [] { "johnny harris", "youtube", "journalism", "maps" }},
        {"jovanovic", new [] { "milan", "dotnet", "software development"}},
        {"jquery", new []{"jquery", "javascript", "frontend"}},
        { "js", new [] { "javascript", "web", "frontend" } },
        { "json", new [] { "json", "serialization", "software" } },
        { "katalon", new [] { "katalon", "testing", "visual studio", "apis" } },
        {"Kotlin", new [] { "Kotlin", "Android development", "mobile apps" }},
        {"kubernates", new []{"docker", "kubernates", "k8", "containers"}},
        {"k8", new []{"docker", "kubernates", "k8", "containers"}},
        {"lambdatest", new [] { "lambdatest", "test", "frontend"}},
        {"linux", new [] { "linux"}},
        {"logrocket", new []{"logrocket", "tech", "frontend", "blog"}},
        { "masstransit", new[] { "masstransit", "pubsub", "queues" } },
        {"macos", new [] { "macos", "apple"}},
        { "mediatr", new[] { "mediatr","cqrs", "software" } },
        {"microos", new [] { "microos", "suse", "opensuse","linux" }},
        {"microservices", new [] { "microservices", "software", "architecture"}},
        {"microsoft", new [] { "microsoft"}},
        {"milan jovanovic", new [] { "milan", "dotnet", "software development"}},
        {"milan jovanović", new [] { "milan", "dotnet", "software development"}},
        {"milanjovano", new [] { "milan", "dotnet", "software development"}},
        { "mobile apps", new [] { "mobile apps", "app development", "iOS", "Android" }},
        { "modular monolith", new [] { "modular monolith", "software", "architecture", "design patterns", "microservices" }},
        { "mongo", new [] { "mongodb", "databases", "nosql" }},
        { "mongodb", new [] { "mongodb", "databases", "nosql" }},
        { "moq", new [] { "moq", "testing", "unit testing" }},
        { "mysql", new [] { "mysql", "databases", "sql" }},
        { "newtonsoft", new [] { "newtonsoft", "json", "serialization", "software" } },
        { "nextjs", new [] { "nextjs", "react", "frontend", "ui" } },
        { "next.js", new [] { "nextjs", "react", "frontend", "ui" } },
        {"nick chapsas", new [] {"nick chapsas", "dotnet", "software development"}},
        { "npm", new [] { "npm", "frontend", "package manager" } },
        {"opensuse", new [] { "suse", "opensuse","linux" }},
        {"oracle", new [] { "oracle"}},
        { "pnpm", new [] { "pnpm","npm", "frontend", "package manager" } },
        { "nuget", new [] { "nuget", "visual studio", "package manager" } },
        {"PHP", new [] { "PHP", "PHP programming", "server-side scripting" }},
        {"playwright", new [] { "playwright", "testing", "frontend", "ui" }},
        { "postgres", new [] { "postgres", "databases", "sql" }},
        { "programming", new [] { "programming", "coding", "software development" }},
        {"proxmox", new []{"virtual machines", "proxmox"}},
        {"Python", new [] { "Python", "Python programming", "scripting" }},
        { "rabbitmq", new[] { "rabbitmq", "pubsub", "queues" } },
        { "react", new [] { "react", "javascript", "frontend" } },
        { "redux", new [] { "redux", "react", "javascript", "frontend" } },
        { "rivian", new [] { "rivian", "evs", "clean", "green" } },
        {"Ruby", new [] { "Ruby", "Ruby programming", "web development" }},
        {"Rust", new [] { "Rust", "systems programming", "high performance" }},
        { "sass", new [] { "sass", "css", "frontend" } },
        { "security", new [] { "cybersecurity", "security", "hacking" } },
        {"selenium", new [] {"selenium", "testing", "frontend"}},
        {"signalr", new []{"signalr", "dotnet", "frontend"}},
        { "software", new [] { "technology", "software", "development" } },
        { "sql", new[] { "sql", "databases" }},
        { "specflow", new[] { "specflow", "testing" }},
        { "stackoverflow", new[] { "stackoverflow" }},
        { "storybook", new[] { "storybook", "frontend", "ui", "tools", "testing" }},
        {"structurizr", new[] {"structurizr", "c4", "diagrams", "software"}},
        {"Swift", new [] { "Swift", "iOS development", "mobile apps" }},
        {"suse", new [] { "suse", "linux"}},
        {"swagger", new [] { "swagger", "apis"}},
        { "tailwind", new [] { "tailwind", "css", "ui", "frontend" } },
        { "technology", new [] { "technology", "software", "development" } },
        { "tesla", new [] { "tesla", "evs", "clean", "green" } },
        {"TypeScript", new [] { "TypeScript", "JavaScript", "static typing" }},
        {"ubuntu", new [] { "ubuntu", "linux"}},
        { "visual studio", new [] {"visual studio"}},
        { "visual studio code", new [] {"vscode"}},
        { "vs code", new [] {"vscode"}},
        {"windows", new [] { "windows", "microsoft"}},
        { ".net", new [] { "csharp", "dotnet" } },
        { "UI/UX design", new [] { "UI/UX design", "user interface", "user experience" }},
        { "unit test", new [] { "testing", "unit testing" }},
        {"vm", new []{"virtual machines"}},
        {"virtual machines", new []{"virtual machines"}},
        { "virtual reality", new [] { "virtual reality", "VR", "immersive experiences" }},
        { "vue", new [] { "vuejs", "javascript", "frontend", "ui" }},
        { "vuejs", new [] { "vuejs","javascript", "frontend", "ui" }},
        { "web development", new [] { "web development", "frontend", "backend", "full-stack" }},
        {"xunit", new []{"xunit", "unit testing", "dotnet"}},
        // CI/CD
        { "bitbucket", new [] { "git", "bitbucket", "ci-cd" }},
        { "continuous integration", new [] { "git", "ci-cd" }},
        { "git", new [] { "git", "github", "version control" } },
        { "github", new []{"git", "github", "ci-cd"}},
        { "gitlab", new [] { "git","gitlab", "ci-cd" }},
        { "gitea", new [] { "git", "gitea", "ci-cd" } },
        { "jenkins", new [] { "jenkins", "ci-cd" }},
        { "sonar", new [] { "sonarqube", "ci-cd", "code quality" }},
        { "sonarqube", new [] { "sonarqube", "ci-cd", "code quality" }},
        { "sonarlint", new [] { "sonarlint", "sonarqube", "ci-cd", "code quality" }},

        // AI
        { "artificial intelligence", new [] {"AI", "machine learning", "deep learning" }},
        { "chatgpt", new [] { "AI", "chatgpt", "gpt", "machine learning", "deep learning" }},
        { "gpt", new [] { "AI", "chatgpt", "gpt", "machine learning", "deep learning" }},

       // Tools & Platforms
       { "AWS", new [] { "AWS", "Amazon Web Services", "cloud computing" }},
       {"facebook", new []{"facebook"}},
       { "Google Cloud", new [] { "Google Cloud", "cloud computing", "GCP" }},
       { "Heroku", new [] { "Heroku", "cloud platform", "PaaS" }},
       { "Jira", new [] { "Jira", "project management", "issue tracking" }},
       { "Magento", new [] { "Magento", "e-commerce", "online store" }},
       { "Shopify", new [] { "Shopify", "e-commerce", "online store" }},
       { "Slack", new [] { "Slack", "team collaboration", "messaging" }},
       { "Trello", new [] { "Trello", "project management", "task organization" }},
       { "WordPress", new [] { "WordPress", "blogging", "content management" }},
       { "Microsoft Azure", new [] { "Azure", "cloud computing", "Azure services" }},
       {"stickeryou", new []{"stickeryou", "stickers"}},
       {"sticker maker", new []{ "sticker maker","stickeryou", "stickers"}},
       {"figma", new [] { "figma", "designing", "frontends" }},
       {"penpot", new [] { "penpot", "figma", "designing", "frontends" }},
       {"sketch", new [] { "sketch", "designing", "frontends" }},
       {"Spotify", new[] {"spotify"}},

        // News
        {"arstechnica", new []{"arstechnica", "tech", "articles", "news"}},
        {"cbc", new [] { "news", "cbc", "canada"}},
        {"cnbc", new [] {"news", "usa", "cnbc"}},
        {"cnn", new [] {"news", "usa", "cnn"}},
        {"ctv", new [] { "news", "ctv", "canada" }},
        {"fivethirtyeight", new [] {"fivethirtyeight", "stats", "analytics", "articles", "sports"}},
        {"globalnews", new [] { "news", "globalnews", "canada", "toronto"}},
        {"nationalpost", new [] { "news", "nationalpost", "canada", "toronto"}},
        {"nytimes", new [] { "news", "nytimes", "usa", "world"}},
        {"thestar", new [] { "news", "thestar", "canada", "toronto"}},
        {"theguardian", new [] { "news", "theguardian", "uk", "world"}},
        {"theringer", new []{"theringer", "sports", "movies", "culture", "articles"}},
        {"tipranks", new []{"investmenting", "stocks"}},
        {"torontosun", new [] { "news", "torontosun", "canada", "toronto"}},
        {"politics", new [] { "politics"}},
        {"wired.com", new [] { "tech", "news", "articles"}},
        


        // Foods & Recipes
        { "allrecipes", new [] { "recipes", "allrecipes"}},
        {"barbecue", new [] {"bbq", "food"}},
        {"barbeque", new [] {"bbq", "food"}},
        {"bbq", new []{"bbq", "food"}},
        { "bonappetit", new [] { "recipes", "bonappetit"}},
        { "brisket", new [] { "beef", "brisket","bbq", "smoking"}},
        { "cooking", new [] { "recipes", "cooking"}},
        { "cookinglight", new [] { "recipes", "cookinglight"}},
        { "delish", new [] { "recipes", "delish"}},
        { "epicurious", new [] { "recipes", "epicurious"}},
        { "foodnetwork", new [] { "recipes", "foodnetwork"}},
        {"grilling", new []{"grilling", "bbqing", "bbq"}},
        {"hot ones", new []{"hot ones", "youtube", "spicy", "hot sauce", "first we feast"}},
        { "intstapot", new [] { "intstapot","slow cooker" }},
        { "one pot", new []{"one pot", "recipes"}},
        { "one-pot", new []{"one pot", "recipes"}},
        { "pulled pork", new [] { "pulled pork", "bbq", "smoking"}},
        { "recipe", new [] {"recipes"}},
        { "recipes", new [] {"recipes"}},
        { "slow cooker", new [] {"slow cooker"}},
        { "slow-cooker", new [] {"slow cooker"}},
        { "traeger", new [] {"traeger", "bbq", "smoking"}},
        { "yummly", new [] { "recipes", "yummly"}},

        // Sports
        { "blue jays", new [] { "blue jays", "sports", "mlb", "toronto"}},
        { "cfl", new [] { "cfl", "football", "sports", "canada"}},
        { "espn", new [] { "espn", "sports", "articles"}},
        { "mlb", new [] { "mlb", "baseball", "sports"}},
        { "nba", new [] { "nba", "basketball", "sports"}},
        { "nfl", new [] { "nfl", "football", "sports"}},
        { "raptors", new [] { "raptors", "basketball", "sports", "nba", "toronto"}},
        { "raptorshq", new [] { "raptorshq", "raptors", "basketball", "sports", "nba", "toronto"}},
        { "sports illustrated", new [] { "sports illustrated", "sports", "articles"}},
        { "theathletic", new [] { "theathletic", "sports", "articles"}},

        // Health & Fitness
        {"cycling", new [] { "cycling"}},
        {"exercise", new [] { "exercise","fitness" }},
        {"exercing", new [] { "exercise", "fitness" }},
        {"fitness", new [] { "fitness"}},
        {"gcn", new [] { "gcn", "cycling"}},
        {"gravel cycling", new [] {"gravel", "cycling"}},
        {"health", new [] { "health"}},
        {"kettlebells", new [] { "kettlebells", "fitness"}},
        {"peloton", new [] { "cycling", "indoorcycling", "peloton"}},
        {"running", new [] { "running", "fitness"}},
        {"stretching", new [] { "stretching", "health", "fitness"}},
        {"stretches", new [] { "stretching", "health", "fitness"}},
        {"wahoo", new [] { "cycling", "indoorcycling", "wahoo"}},
        {"weightlifting", new [] { "weightlifting", "fitness"}},
        {"weights", new [] { "weights", "fitness"}},
        {"workout", new [] { "workout", "fitness"}},
        {"workouts", new [] { "workouts", "fitness"}},
        {"yoga", new [] { "yoga", "fitness"}},
        {"zwift", new [] { "cycling", "indoorcycling", "zwift"}},
        
        // Movies & Entertainment
        {"marvel", new []{ "marvel", "movies", "comics"}},
        {"starcraft", new []{"starcraft", "blizzard", "games", "strategy"}},
        {"warcraft", new []{ "warcraft", "blizzard", "games", "mmorpg"}},

        // Misc
        {"toronto", new [] { "toronto", "ontario", "canada"}},
        {"vancouver", new [] { "vancouver", "british columbia","canada"}},
        {"fool.ca", new [] { "fool", "canada", "investing", "stocks"}},
        {"joe rogan", new [] { "joe rogan", "podcast", "mma", "comedian"}},

        // Videos
        {"youtube", new []{"youtube", "video"}},

        // Space & Science
        {"nasa", new []{ "nasa", "space"}},
        {"spacex", new []{ "spacex", "nasa", "space", "elon"}},
        {"space-x", new []{ "spacex", "nasa", "space", "elon"}},

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

        var title = $"{link.MetaData?.Title ?? ""} {link.MetaData?.OgTitle ?? ""}";
        var description = $"{link.MetaData?.Description ?? ""} {link.MetaData?.OgDescription ?? ""}";
        var keywords = link.MetaData?.Keywords ?? "";

        if (!string.IsNullOrWhiteSpace(title))
        {
            // This isn't proper ... this is to keep the ExecuteAsync contract.
            // Eventually this will call some other service/process to get the tags
            var titleTags = _tagSuggestions.Where(x => title.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();

            tags = tags.Union(titleTags).ToArray();
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            var descriptionTags = _tagSuggestions.Where(x => description.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();

            tags = tags.Union(descriptionTags).ToArray();
        }

        if (!string.IsNullOrWhiteSpace(keywords))
        {
            var keywordTags = _tagSuggestions.Where(x => keywords.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();

            tags = tags.Union(keywordTags).ToArray();
        }

        if (!string.IsNullOrWhiteSpace(link.Domain))
        {
            var domainTags = _tagSuggestions.Where(x => link.Domain.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();

            tags = tags.Union(domainTags).ToArray();
        }

        //if (link.MetaData != null)
        //{
        //    var keywordTags = await Task.Run(() =>
        //    {
        //        return _tagSuggestions.Where(x => title.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();
        //    }, token);

        //    var descriptionTags = await Task.Run(() =>
        //    {
        //        return _tagSuggestions.Where(x => title.ToLower().Contains(x.Key)).SelectMany(x => x.Value)?.ToArray() ?? Array.Empty<string>();
        //    }, token);

        //    tags = (titleTags.Union(keywordTags).Concat(descriptionTags)).Distinct().ToArray();
        //}

        if (tags.Length > 0)
        {
            var x = true;
        }

        link = link with { Tags = tags.Distinct().Select(t => t.ToLower()).ToArray() };
        link = link with { State = QueuedStates.TaggingCompleted };

        return (true, "Tagging Completed", link);
    }
}