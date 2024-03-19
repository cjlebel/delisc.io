namespace Deliscio.Modules.QueuedLinks.Common.Enums;

public sealed class QueuedStates
{
    public class State
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public State(int id, string name, int order)
        {
            Id = id;
            Name = name;
            Order = order;
        }
    }


    /// <summary>
    /// The state when a link is submitted
    /// </summary>
    public static readonly State New = new(1, "New", 1);

    /// <summary>
    /// The state when an existing link is being updated
    /// </summary>
    public static readonly State Update = new(2, "Update", 2);

    /// <summary>
    /// The state when a link is being checked to see if it already exists, or if it's valid
    /// </summary>
    public static readonly State Verifying = new(3, "Verifying Exists", 2);
    public static readonly State VerifyingCompleted = new(4, "Verifying Completed", 3);

    /// <summary>
    /// The state when a link is being scraped for meta data
    /// </summary>
    public static readonly State FetchingData = new(5, "Fetching Site Data", 5);
    public static readonly State FetchingDataCompleted = new(6, "Fetching Site Data Completed", 6);

    /// <summary>
    /// The state when a link is being tagged based on it's meta
    /// </summary>
    public static readonly State Tagging = new(7, "Tagging", 7);
    public static readonly State TaggingCompleted = new(8, "Tagging Completed", 8);

    /// <summary>
    /// The state when a link has been found to already exist
    /// </summary>
    public static readonly State Error = new(700, "Error", 700);

    /// <summary>
    /// The state when a link has been found to already exist
    /// </summary>
    public static readonly State Exists = new(800, "Exists", 800);

    /// <summary>
    /// If a link is rejected, it will be in this state
    /// </summary>
    public static readonly State Rejected = new(900, "Rejected", 900);

    /// <summary>
    /// The state when a link has finished being processed
    /// </summary>
    public static readonly State Finished = new(999, "Finished", 999);

    private readonly State[] _newStates = [New, Verifying, VerifyingCompleted, FetchingData, FetchingDataCompleted, Tagging, TaggingCompleted, Rejected, Finished];
    private readonly State[] _updateStates = [Update, FetchingData, FetchingDataCompleted, Tagging, TaggingCompleted, Rejected, Finished];

    /// <summary>
    /// List of all states related to when a new link is submitted
    /// </summary>
    public State[] NewStatesList()
    {
        return _newStates.OrderBy(s => s.Order).ToArray();
    }

    /// <summary>
    /// List of all states related to an updating an existing link
    /// </summary>
    public State[] UpdateStatesList()
    {
        return _updateStates.OrderBy(s => s.Order).ToArray();
    }
}