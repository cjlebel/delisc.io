using Ardalis.GuardClauses;
using Deliscio.Modules.Links.Domain.LinkTags;
using Deliscio.Modules.BuildingBlocks.Domain.ValueObjects;
using Deliscio.Modules.Links.Domain.Events;
using Deliscio.Modules.Links.Infrastructure.Data.Entities;

using Habanerio.Core.BuildingBlocks.Domain;

using MongoDB.Bson;

namespace Deliscio.Modules.Links.Domain.Links;
public sealed class Link : Entity<ObjectId>, IAggregateRoot<LinkId, ObjectId>
{
    #region - Properties -
    public LinkId Id { get; private init; }

    public LinkTitle Title { get; private set; }

    public LinkDescription Description { get; private set; }

    public LinkDomain Domain { get; private set; }

    public LinkImageUrl ImageUrl { get; private set; }

    /// <summary>
    /// Meta keywords from the actual page (if they exist).
    /// This cannot (should not) be changed after the fact.
    /// </summary>
    public IReadOnlyCollection<string> Keywords { get; private set; }

    public bool IsActive { get; private set; }

    public bool IsFlagged { get; private set; }

    public int SavesCount { get; private set; }

    public int LikesCount { get; private set; }

    public LinkTagCollection TagsCollection { get; private set; }

    public LinkUrl Url { get; private init; }


    public CreatedById CreatedById { get; private set; }

    public DateTime DateCreated { get; private set; }


    public UpdatedById UpdatedById { get; private set; }

    public DateTimeOffset? DateUpdated { get; private set; }


    public bool IsDeleted { get; private set; }

    public DeletedById DeletedById { get; private set; }

    public DateTimeOffset? DateDeleted { get; private set; }

    #endregion

    private Link(LinkId id,
        LinkTitle title,
        LinkDescription description,
        LinkDomain domain,
        LinkImageUrl imageUrl,
        string keywords,
        LinkTagCollection tagsCollection,
        LinkUrl url,

        bool isActive,
        bool isFlagged,
        int savesCount,
        int likesCount,

        CreatedById createdBy, DateTimeOffset dateCreated,

        UpdatedById? updatedById = null, DateTimeOffset? dateUpdated = null,

        bool isDeleted = false, DateTimeOffset? dateDeleted = null, DeletedById? deletedById = null
        )
    {
        Id = id;
        Title = title;
        Description = description;
        Domain = domain;
        ImageUrl = imageUrl;
        Keywords = (
            !string.IsNullOrWhiteSpace(keywords) ? 
            keywords?.Split(',')
                .Select(k => k.Trim())
                .ToArray() : []
            ) ?? [];
        TagsCollection = tagsCollection;
        Url = url;

        IsActive = isActive;
        IsFlagged = isFlagged;

        SavesCount = savesCount;
        LikesCount = likesCount;

        CreatedById = createdBy;
        DateCreated = dateCreated.LocalDateTime;

        DateUpdated = dateUpdated;
        UpdatedById = updatedById ?? UpdatedById.Create(ObjectId.Empty);

        IsDeleted = isDeleted;
        DateDeleted = dateDeleted;
        DeletedById = deletedById ?? DeletedById.Create(ObjectId.Empty);
    }

    public static Link New(
        LinkTitle title, 
        LinkDescription description, 
        LinkUrl url, 
        LinkImageUrl imageUrl, 
        CreatedById createdBy, 
        DateTimeOffset dateCreated, 
        LinkTag[]? tags = null, 
        string keywords = "")
    {
        Guard.Against.Null(title, nameof(title));
        Guard.Against.Null(url, nameof(url));
        Guard.Against.Null(createdBy, nameof(createdBy));

        return new Link(
            LinkId.Create(ObjectId.GenerateNewId()),
            title,
            description,
            LinkDomain.FromUrl(url.Value),
            imageUrl,
            keywords ?? string.Empty,
            new LinkTagCollection(tags ?? Enumerable.Empty<LinkTag>()),
            url,

            true,
            false,
            0,
            0,

            createdBy,
            dateCreated.LocalDateTime
        );
    }

    public void Delete(DeletedById deletedById, DateTimeOffset? dateDeleted = null)
    {
        Guard.Against.Null(deletedById, nameof(deletedById));

        if (!IsDeleted)
        {
            this.IsDeleted = true;
            this.DeletedById = deletedById;
            this.DateDeleted = dateDeleted ?? DateTimeOffset.UtcNow;

            this.IsActive = false;

            SetUpdateInfo(UpdatedById.Create(deletedById.value), this.DateDeleted);

            //this.AddDomainEvent(new LinkDeletedDomainEvent(this.LinkId.Value, this.DeletedByUserId.Value));
        }
    }

    public void UnDelete(UpdatedById updatedById)
    {
        Guard.Against.Null(updatedById, nameof(updatedById));

        if (this.IsDeleted)
        {
            this.IsDeleted = false;
            this.DeletedById = DeletedById.Create(ObjectId.Empty);
            this.DateDeleted = null;

            SetUpdateInfo(updatedById);

            //this.AddDomainEvent(new LinkUndeletedDomainEvent(this.LinkId.Value, this.UpdatedByUserId.Value));
        }
    }

    public void Edit(LinkTitle title, LinkDescription description, LinkTag[] newTags, UpdatedById updatedByUserId)
    {
        Guard.Against.Null(title, nameof(title));
        Guard.Against.Null(updatedByUserId, nameof(updatedByUserId));

        if (!IsDeleted)
        {
            // This should never happen, as LinkTitle should have thrown an error prior to this
            if (string.IsNullOrWhiteSpace(title.Value))
                throw new ArgumentException("Title cannot be empty", nameof(title));
            
            this.Title = title;
            this.Description = description;

            //this.TagsCollection = new LinkTagCollection(tagsCollection);

            MergeTags(newTags);

            SetUpdateInfo(updatedByUserId);

            //this.AddDomainEvent(new LinkUpdatedDomainEvent(this.LinkId.Value, title.Value, description.Value, this.TagsCollection., this.UpdatedByUserId.Value));
        }
    }

    /// <summary>
    /// Sets (or updates) the total number of likes for on all user links, based on this individual link
    /// </summary>
    /// <param name="newLikesCount">The new value for the Links Count</param>
    /// <param name="updatedByUserId"></param>
    public void SetLikesCount(int newLikesCount, UpdatedById updatedByUserId)
    {
        Guard.Against.Null(updatedByUserId, nameof(updatedByUserId));
        Guard.Against.Negative(newLikesCount, nameof(newLikesCount));

        if (!IsDeleted)
        {
            var oldValue = LikesCount;
            this.LikesCount = newLikesCount;

            SetUpdateInfo(updatedByUserId);

            this.AddDomainEvent(new LinkLikesCountChangedDomainEvent(this.Id.Value, oldValue, newLikesCount, updatedByUserId.Value));
        }
    }

    /// <summary>
    /// Sets (or updates) the total number of times users have saved this link as their own
    /// </summary>
    /// <param name="newSavesCount">The new value for the Links Count</param>
    /// <param name="updatedByUserId"></param>
    public void SetSavesCount(int newSavesCount, UpdatedById updatedByUserId)
    {
        Guard.Against.Null(updatedByUserId, nameof(updatedByUserId));
        Guard.Against.Negative(newSavesCount, nameof(newSavesCount));

        if (!IsDeleted)
        {
            var oldValue = SavesCount;
            this.SavesCount = newSavesCount;

            SetUpdateInfo(updatedByUserId);

            this.AddDomainEvent(new LinkSavedCountChangedDomainEvent(this.Id.Value, oldValue, newSavesCount, updatedByUserId.Value));
        }
    }


    public void SetActiveState(bool isActive, UpdatedById updatedByUserId)
    {
        Guard.Against.Null(updatedByUserId, nameof(updatedByUserId));

        if (!IsDeleted)
        {
            this.IsActive = isActive;

            SetUpdateInfo(updatedByUserId);

            this.AddDomainEvent(new LinkActiveStatusChangedDomainEvent(this.Id.Value, true, updatedByUserId.Value));
        }
    }

    public void SetFlaggedState(bool isFlagged, UpdatedById updatedByUserId)
    {
        Guard.Against.Null(updatedByUserId, nameof(updatedByUserId));

        if (!IsDeleted && IsFlagged != isFlagged)
        {
            this.IsFlagged = isFlagged;

            SetUpdateInfo(updatedByUserId);

            //this.AddDomainEvent(new LinkFlaggedStatusChangedDomainEvent(this.LinkId.Value, true, updatedByUserId.Value));
        }
    }

    /// <summary>
    /// Updates the existing tagsCollection with the provided ones.
    /// If a tag is not in the new list, it will be removed.
    /// </summary>
    /// <param name="newTags"></param>
    public void UpdateTags(string[] newTags)
    {
        UpdateTags(newTags.Select(t => LinkTag.New(t)).ToArray());
    }

    public void UpdateTags(LinkTag[] newTags)
    {
        if(newTags is null || newTags.Length == 0)
            return;

        MergeTags(newTags);

        //this.AddDomainEvent(new LinkTagsChangedDomainEvent(this.LinkId.Value, newTags.Select(t => t.Name).ToArray(), this.UpdatedByUserId.Value));
    }

    /// <summary>
    /// Compares the existing list of tagsCollection with the new list and adds/removes tagsCollection as needed
    /// </summary>
    /// <param name="newTags">The new set of tagsCollection</param>
    private void MergeTags(LinkTag[] newTags)
    {
        var existingTags = TagsCollection.Tags.ToArray();

        // Trim the names of each tag
        var trimmedTags = newTags.Select(t => LinkTag.Create(t.Name.Trim().ToLower(), t.Count, t.Weight)).ToArray();

        // Remove any tags that are not in the list of tags that were passed in
        for (var i = 0; i < existingTags.Length - 1; i++)
        {
            var existingTag = existingTags[i];

            // If a previously existing tag is not in the new list, then remove it from the main list
            if (!trimmedTags.Any(t => t.Name.Equals(existingTag.Name)))
            {
                TagsCollection.Delete(existingTag);
            }
        }

        // Add/update the tags with the new list
        this.TagsCollection.Add(trimmedTags);
    }


    /// <summary>
    /// Create a new link from an existing entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Link Map(LinkEntity entity)
    {
        if (entity is null)
            return null;

        // SubmittedById was originally used. So in cases where it was previously set, us it for the CreatedById
        var createdBy = entity.CreatedById != ObjectId.Empty ? CreatedById.Create(entity.CreatedById) : CreatedById.Create(entity.SubmittedById);

        return new(
            LinkId.Create(entity.Id),
            new LinkTitle(entity.Title),
            new LinkDescription(entity.Description),
            new LinkDomain(entity.Domain),
            new LinkImageUrl(entity.ImageUrl),
            string.Join(',', entity.Keywords),
            new LinkTagCollection(entity.Tags
                .Where(t=>!string.IsNullOrWhiteSpace(t.Name))
                .Select(t => LinkTag.Map(t))
                .ToArray()),

            new LinkUrl(entity.Url),

            entity.IsActive,
            entity.IsFlagged,
            entity.SavesCount,
            entity.LikesCount,

            createdBy,
            entity.DateCreated,

            UpdatedById.Create(entity.UpdatedById),
            entity.DateUpdated,

            entity.IsDeleted,
            entity.DateDeleted,
            DeletedById.Create(entity.DeletedById)
        );
    }

    private void SetUpdateInfo(UpdatedById updatedByUserId, DateTimeOffset? dateUpdated  = null)
    {
        Guard.Against.Null(updatedByUserId, nameof(updatedByUserId));

        this.DateUpdated = dateUpdated ?? DateTimeOffset.UtcNow;
        this.UpdatedById = updatedByUserId;
    }
}
