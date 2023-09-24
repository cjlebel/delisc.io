namespace Deliscio.Modules.QueuedLinks.Common.Models;

public sealed record UsersData
{
    public string? Description { get; set; }

    public string[]? Tags { get; set; }

    public string? Title { get; set; }

    public static UsersData Create(string title = "", string description = "", string[]? tags = default)
    {
        var rslt = new UsersData()
        {
            Description = description,
            Tags = tags ?? Array.Empty<string>(),
            Title = title
        };

        return rslt;
    }
}