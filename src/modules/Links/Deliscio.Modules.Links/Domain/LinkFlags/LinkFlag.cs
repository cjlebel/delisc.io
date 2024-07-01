using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;

namespace Deliscio.Modules.Links.Domain.LinkFlags;

/// <summary>
/// Represents an individual flag that a user has submitted for a link.
/// </summary>
public class LinkFlag
{
    public FlagId Id { get; set; }

    public LinkId LinkId { get; set; }

    public CreatedById CreatedById { get; set; }

    public DateTimeOffset DateCreated { get; set; }

    public UpdatedById? UpdatedById { get; set; }

    public DateTimeOffset? DateUpdated { get; set; }

    public string Reason { get; set; }

    public string Feedback { get; set; }

    public FlagState State { get; set; }

    public LinkFlag(FlagId id, LinkId linkId, CreatedById createdById, DateTimeOffset dateCreated, UpdatedById? updatedById, DateTimeOffset? dateUpdated, string reason, string feedback, FlagState state)
    {
        Id = id;
        LinkId = linkId;
        CreatedById = createdById;
        DateCreated = dateCreated;
        UpdatedById = updatedById;
        DateUpdated = dateUpdated;
        Reason = reason;
        Feedback = feedback;
        State = state;
    }

}