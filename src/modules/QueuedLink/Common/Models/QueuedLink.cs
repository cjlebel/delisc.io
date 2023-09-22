using Deliscio.Modules.QueuedLinks.Common.Enums;

namespace Deliscio.Modules.QueuedLinks.Common.Models;

public record QueuedLink
{
    public string Url { get; } = default!;
    public Guid SubmittedById { get; }
    public string[]? Tags { get; }
    public string UsersTitle { get; } = default!;
    public string UsersDescription { get; } = default!;
    public QueueStates State { get; } = QueueStates.New;

    public QueuedLink(string url, string submittedById, string usersTitle = "", string usersDescription = "",
        string[]? tags = default) : this(url, new Guid(submittedById), usersTitle, usersDescription, tags) { }

    public QueuedLink(string url, Guid submittedById, string usersTitle = "", string usersDescription = "",
        string[]? tags = default)
    {
        Url = url;
        SubmittedById = submittedById;
        Tags = tags;
        UsersTitle = usersTitle;
        UsersDescription = usersDescription;
    }
}