//using Ardalis.GuardClauses;
//using Deliscio.Core.Models;
//using Deliscio.Modules.Links.Common.Interfaces;
//using Deliscio.Modules.Links.Common.Models;
//using Deliscio.Modules.Links.Common.Models.Requests;
//using Deliscio.Modules.Links.Data.Entities;
//using Deliscio.Modules.Links.Mappers;
//using Microsoft.Extensions.Logging;
//using MongoDB.Bson;

//namespace Deliscio.Modules.Links;

//public class LinksAdminService : LinksBaseService<LinksAdminService>, ILinksAdminService
//{
//    public LinksAdminService(ILinksRepository repository, ILogger<LinksAdminService> logger) : base(repository, logger) { }

//    /// <summary>
//    /// Gets a PagedResults of LinkItems based on the find request criteria.
//    /// </summary>
//    public Task<PagedResults<LinkItem>> FindAsync(FindLinksRequest request, CancellationToken token = default)
//    {
//        return base.FindLinksAsync(request.SearchTerm, request.TagsCollection, request.Domain, request.PageNo, request.PageSize, request.Offset, request.IsActive, request.IsFlagged, request.IsDeleted, token);
//    }

//    #region - CRUD -

//    public async Task<string> AddAsync(Link link, CancellationToken token = default)
//    {
//        Guard.Against.Null(link);
//        Guard.Against.NullOrWhiteSpace(link.Url);
//        Guard.Against.NullOrWhiteSpace(link.Title);
//        Guard.Against.NullOrWhiteSpace(link.CreatedByUserId);

//        var entity = Mapper.Map(link);

//        if (entity == null)
//        {
//            Logger.LogError("Unable to map link to entity");
//            return string.Empty;
//        }

//        await LinksRepository.AddAsync(entity, token);

//        return entity.LinkId.ToString();
//    }

//    /// <summary>
//    /// Simplest way to add a link to the central link repository.
//    /// </summary>
//    /// <param name="url"></param>
//    /// <param name="title"></param>
//    /// <param name="submittedById"></param>
//    /// <param name="tags"></param>
//    /// <param name="token"></param>
//    /// <returns></returns>
//    public async Task<Guid> AddAsync(string url, string title, string submittedById, string[]? tags = default, CancellationToken token = default)
//    {
//        Guard.Against.NullOrWhiteSpace(url);
//        Guard.Against.NullOrWhiteSpace(title);
//        Guard.Against.NullOrWhiteSpace(submittedById);

//        var entity = LinkEntity.Create(url, title, submittedById, tags);

//        await LinksRepository.AddAsync(entity, token);

//        return Guid.Parse(entity.LinkId.ToString());
//    }

//    public async Task<bool> DeleteAsync(string linkId, string deletedById, CancellationToken token = default)
//    {
//        Guard.Against.NullOrWhiteSpace(linkId);
//        Guard.Against.NullOrWhiteSpace(deletedById);

//        try
//        {
//            var link = await LinksRepository.GetAsync(ObjectId.Parse(linkId), token);

//            if (link is null)
//                return false;

//            // Temporarily mark the link as deleted
//            // Have task delete it at a later time
//            link.IsDeleted = true;
//            link.DeletedByUserId = ObjectId.Parse(deletedById);

//            link.IsActive = false;
//            link.UpdatedByUserId = ObjectId.Parse(deletedById);
//            link.DateUpdated = DateTime.UtcNow;

//            await LinksRepository.UpdateAsync(link, token);

//            // Log

//            return true;
//        }
//        catch (Exception e)
//        {
//            return false;
//        }
//    }

//    public async ValueTask<(bool IsSuccess, string Message)> UpdateLinkAsync(string updatedById,
//    string id, string title, string description, bool isActive, string[]? tags = default, CancellationToken token = default)
//    {
//        if (string.IsNullOrWhiteSpace(updatedById))
//            return (false, "Must have a User LinkId to Update this Link");

//        if (string.IsNullOrWhiteSpace(id))
//            return (false, "Must have a Link LinkId to Update this Link");

//        if (string.IsNullOrWhiteSpace(title))
//            return (false, "Must have a Title to Update this Link");

//        var linkEntity = await LinksRepository.GetAsync(ObjectId.Parse(id.ToString()), token);

//        if (linkEntity == null)
//            return (false, "Link not found");

//        linkEntity.IsActive = isActive;
//        linkEntity.Title = title;
//        linkEntity.Description = description;
//        linkEntity.TagsCollection = tags?.Select(LinkTagEntity.Create).Distinct().ToList() ?? [];
//        linkEntity.DateUpdated = DateTime.UtcNow;

//        try
//        {
//            await LinksRepository.UpdateAsync(linkEntity, token);
//        }
//        catch (Exception e)
//        {
//            return (false, e.Message);
//        }

//        return (true, string.Empty);
//    }

//    #endregion

//    private async Task UpdateLinkAsync(LinkEntity linkEntity, string[]? tags = default, CancellationToken token = default)
//    {
//        Guard.Against.Null(linkEntity);
//        Guard.Against.NullOrWhiteSpace(linkEntity.Url);
//        Guard.Against.NullOrWhiteSpace(linkEntity.Title);
//        Guard.Against.NullOrEmpty(linkEntity.SubmittedById.ToString());

//        if (tags is { Length: > 0 })
//        {
//            tags = tags.Distinct().ToArray();

//            var linkTagNames = linkEntity.TagsCollection.Select(t => t.Name).ToArray();

//            var existingTagNames = tags.Where(t => linkTagNames.Contains(t)).ToArray();
//            var nonExistingTagNames = tags.Where(t => !linkTagNames.Contains(t)).ToArray();

//            // If the tag already exists, then just increment the count
//            foreach (var existingTagName in existingTagNames)
//            {
//                var t = linkEntity.TagsCollection.First(t => t.Name == existingTagName);
//                t.Count++;
//            }

//            // If the tag doesn't exist, then add it to the existing tags
//            linkEntity.TagsCollection.AddRange(nonExistingTagNames.Select(LinkTagEntity.Create));

//            await LinksRepository.UpdateAsync(linkEntity, token);
//        }
//    }
//}