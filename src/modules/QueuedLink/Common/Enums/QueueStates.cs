namespace Deliscio.Modules.QueuedLinks.Common.Enums;

public class QueueStates
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Order { get; set; }

    public QueueStates(int id, string name, int order)
    {
        Id = id;
        Name = name;
        Order = order;
    }

    public static readonly QueueStates New = new(1, "New", 1);

    public static readonly QueueStates CheckingIfExists = new(2, "Checking If Exists", 2);
    public static readonly QueueStates CheckingIfExistsCompleted = new(3, "Checking If Exists", 3);

    public static readonly QueueStates NewScraping = new(4, "Checking If Exists", 2);
    public static readonly QueueStates NewScrapingCompleted = new(5, "Checking If Exists", 3);

    public static readonly QueueStates NewTagging = new(4, "Checking If Exists", 2);
    public static readonly QueueStates NewTaggingCompleted = new(5, "Checking If Exists", 3);

    public static readonly QueueStates NewFinished = new(999, "Finished", 999);

    public static readonly QueueStates Update = new(1, "Update", 1);

    public static readonly QueueStates UpdateScraping = new(4, "Checking If Exists", 2);
    public static readonly QueueStates UpdateScrapingCompleted = new(5, "Checking If Exists", 3);

    public static readonly QueueStates UpdateTagging = new(4, "Checking If Exists", 2);
    public static readonly QueueStates UpdateTaggingCompleted = new(5, "Checking If Exists", 3);

    public static readonly QueueStates UpdateFinished = new(999, "Finished", 999);

    /// <summary>
    /// List of all states related to a new link
    /// </summary>
    public QueueStates[] ListNewStates => new[] { New, CheckingIfExists, CheckingIfExistsCompleted, NewScraping, NewScrapingCompleted, NewTagging, NewTaggingCompleted, NewFinished };

    /// <summary>
    /// List of all states related to an updating an existing link
    /// </summary>
    public QueueStates[] ListUpdateStates => new[] { Update, UpdateScraping, UpdateScrapingCompleted, UpdateTagging, UpdateTaggingCompleted, UpdateFinished };
}