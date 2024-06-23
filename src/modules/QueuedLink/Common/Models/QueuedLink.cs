using Deliscio.Modules.QueuedLinks.Common.Enums;

namespace Deliscio.Modules.QueuedLinks.Common.Models;

public record QueuedLink
{
    public string Url { get; set; }

    public string SubmittedById { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Domain { get; set; }

    public string LinkId { get; set; } = string.Empty;

    public string[]? Tags { get; set; }

    public MetaData? MetaData { get; set; }

    public UsersData? UsersData { get; set; }

    public QueuedStates.State State { get; set; } = QueuedStates.New;

    public DateTimeOffset? DateLastFetched { get; set; }

    //NOTE: This () is needed for deserialization :(
    public QueuedLink() { }

    //public QueuedLink(string url, string submittedById, string usersTitle = "", string usersDescription = "",
    //    string[]? tags = default) : this(new Uri(url), new Guid(submittedById), usersTitle, usersDescription, tags) { }

    //public QueuedLink(Uri url, Guid submittedById, string usersTitle = "", string usersDescription = "",
    //    string[]? tags = default)
    //{
    //    Url = url.OriginalString;
    //    SubmittedById = submittedById;
    //    Tags = tags;
    //    UsersTitle = usersTitle;
    //    UsersDescription = usersDescription;
    //}

    /// <summary>
    /// Creates an instance of a QueuedLink
    /// </summary>
    /// <param name="url">The url of the page to process</param>
    /// <param name="submittedById">The id of the user who submitted the link</param>
    /// <param name="usersData">The data that the user would like to use for their version of the link</param>
    /// <returns></returns>
    public static QueuedLink Create(Uri url, string submittedById, UsersData? usersData)
    {
        var link = new QueuedLink
        {
            Url = url.OriginalString,
            SubmittedById = submittedById,
            UsersData = usersData ?? new UsersData()
        };

        return link;
    }
}