using Deliscio.Modules.QueuedLinks.Common.Enums;

namespace Deliscio.Modules.QueuedLinks.Common.Models;

public class QueuedLink
{
    public string Url { get; set; }

    public Guid SubmittedById { get; set; }

    public string Description { get; set; } = "";

    public string[]? Tags { get; }

    public string Title { get; set; } = "";

    public string UsersTitle { get; }

    public string UsersDescription { get; }

    public QueuedStates.State State { get; set; } = QueuedStates.New;

    //NOTE: This () is needed for deserialization :(
    public QueuedLink() { }

    public QueuedLink(string url, string submittedById, string usersTitle = "", string usersDescription = "",
        string[]? tags = default) : this(new Uri(url), new Guid(submittedById), usersTitle, usersDescription, tags) { }

    public QueuedLink(Uri url, Guid submittedById, string usersTitle = "", string usersDescription = "",
        string[]? tags = default)
    {
        Url = url.OriginalString;
        SubmittedById = submittedById;
        Tags = tags;
        UsersTitle = usersTitle;
        UsersDescription = usersDescription;
    }
}