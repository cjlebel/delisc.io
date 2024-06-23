//using System.Linq.Expressions;
//using System.Net;
//using Ardalis.GuardClauses;
//using Deliscio.Core.Data.Mongo;
//using Deliscio.Core.Models;
//using Deliscio.Modules.Links.Common.Interfaces;
//using Deliscio.Modules.Links.Common.Models;
//using Deliscio.Modules.Links.Common.Models.Requests;
//using Deliscio.Modules.Links.Data.Entities;
//using Deliscio.Modules.Links.Mappers;
//using FluentResults;
//using Microsoft.Extensions.Logging;
//using Structurizr.Annotations;

//namespace Deliscio.Modules.Links;

//[Component(Description = "Deliscio service that deals with the central Links", Technology = "C#")]
//[UsedBySoftwareSystem("Deliscio.Apis.WebApi", Description = "Links Service")]
//public sealed class LinksService : LinksBaseService<LinksService>, ILinksService
//{
//    public LinksService(ILinksRepository linksRepository, ILogger<LinksService> logger) : base(linksRepository, logger) { }


//    public Task<PagedResults<LinkItem>> FindAsync(FindLinksRequest request, CancellationToken token = default)
//    {
//        return base.FindLinksAsync(request.SearchTerm, request.TagsCollection, request.Domain, request.PageNo, request.PageSize, request.Offset, request.IsActive, request.IsFlagged, request.IsDeleted, token);
//    }

//    /// <summary>
//    /// Gets a collection of links from the central link repository.
//    /// </summary>
//    /// <param name="pageNo">The number of the page of results to be returned</param>
//    /// <param name="pageSize">The number of results per page</param>
//    /// <param name="token"></param>
//    /// <returns></returns>
//    public override Task<PagedResults<LinkItem>> GetAsync(int pageNo = 1, int? pageSize = default, CancellationToken token = default)
//    {
//        var newPageSize = pageSize ?? DEFAULT_LINKS_PAGE_SIZE;

//        Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

//        return base.GetAsync(pageNo, pageSize, token);
//    }

//    public override async Task<IEnumerable<LinkItem>> GetByIdsAsync(IEnumerable<string> linkIds, CancellationToken token = default)
//    {
//        var rslts = await LinksRepository.GetAsync(linkIds.ToObjectIds(), token);

//        var links = Mapper.Map<LinkItem>(rslts);

//        return links;
//    }

//    /// <summary>
//    /// Gets a collection of links from the central link repository by their domain name
//    /// </summary>
//    /// <param name="domain">Gets a collection of links by their domain name (eg: github.com)</param>
//    /// <param name="pageNo">The number of the page of results to be returned</param>
//    /// <param name="pageSize">The number of results per page</param>
//    /// <param name="token"></param>
//    /// /// <exception cref="ArgumentNullException">If the domain is null or empty</exception>
//    /// <returns></returns>
//    //public override async Task<PagedResults<LinkItem>> GetLinksByDomainAsync(string domain, int pageNo = 1, int? pageSize = default, CancellationToken token = default)
//    //{
//    //    Guard.Against.NullOrWhiteSpace(domain);

//    //    var newPageSize = pageSize ?? DEFAULT_LINKS_PAGE_SIZE;

//    //    Guard.Against.NegativeOrZero(newPageSize, message: $"{nameof(pageSize)} must be greater than zero");

//    //    var rslts = await LinksRepository.GetLinksByDomainAsync(domain, pageNo, newPageSize, token);

//    //    var links = Mapper.Map<LinkItem>(rslts.Results);

//    //    return new PagedResults<LinkItem>(links, pageNo, newPageSize, rslts.Count);
//    //}

//    ///// <summary>
//    ///// Gets a collection of links from the central link repository that contain all of the specified tags, 
//    ///// </summary>
//    ///// <param name="tags">A collection of tags for which each result that is returned must contain</param>
//    ///// <param name="pageNo">The number of the page of results to be returned</param>
//    ///// <param name="pageSize">The number of results per page</param>
//    ///// <param name="token"></param>
//    ///// <returns></returns>
//    ///// <exception cref="NotImplementedException"></exception>
//    //public override Task<PagedResults<LinkItem>> GetLinksByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int? pageSize = default, CancellationToken token = default)
//    //{
//    //    return GetLinksByTagsAsync(tags, pageNo, pageSize, token);
//    //}

//    /// <summary>
//    /// Gets a link by its Url.
//    /// This is useful when you want to check if a link has already been submitted but you don't know its LinkId.
//    /// </summary>
//    /// <param name="url">The url to use to perform the search</param>
//    /// <param name="token">The cancellation token</param>
//    /// <returns></returns>
//    public override async Task<Link?> GetByUrlAsync(string url, CancellationToken token = default)
//    {
//        Guard.Against.NullOrWhiteSpace(url);

//        var result = await LinksRepository.GetLinkByUrlAsync(url, token);

//        return result is null ? null : Mapper.Map(result);
//    }



//    //public async Task<string> SubmitLinkAsync(string url, Guid submittedByUserId, string[]? tags = default, CancellationToken token = default)
//    //{
//    //    Guard.Against.NullOrWhiteSpace(url);
//    //    Guard.Against.NullOrEmpty(submittedByUserId);

//    //    var linkEntity = await LinksRepository.GetLinkByUrlAsync(url, token);

//    //    // If the link already exists in the central link repository, then we just need to add/update the tags for it
//    //    // Then assign to the user who submitted it
//    //    if (linkEntity == null)
//    //        throw new NotImplementedException("The ability to add a new link from here does not exist yet. It's in the Queue service");
//    //    else
//    //        await UpdateExistingLinkAsync(linkEntity, tags, token);

//    //    //await LinksRepository.AddAsync(link, token);

//    //    //return link.LinkId;

//    //    return linkEntity.LinkId.ToString();
//    //}

//    #region - CRUD -


//    public async Task<string> AddAsync(Link link, CancellationToken token = default)
//    {
//        Guard.Against.Null(link);
//        Guard.Against.NullOrWhiteSpace(link.Url);
//        Guard.Against.NullOrWhiteSpace(link.Title);
//        Guard.Against.NullOrEmpty(link.CreatedByUserId);

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
//    public async Task<string> AddAsync(string url, string title, string submittedById, string[]? tags = default, CancellationToken token = default)
//    {
//        Guard.Against.NullOrWhiteSpace(url);
//        Guard.Against.NullOrWhiteSpace(title);
//        Guard.Against.NullOrEmpty(submittedById);

//        var entity = LinkEntity.Create(url, title, submittedById, tags);

//        await LinksRepository.AddAsync(entity, token);

//        return entity.LinkId.ToString();
//    }

//    public async Task<(bool IsSuccess, string Message)> DeleteAsync(string linkId, string deletedById, CancellationToken token = default)
//    {
//        Guard.Against.NullOrEmpty(linkId);
//        Guard.Against.NullOrEmpty(deletedById);

//        try
//        {
//            var link = await LinksRepository.GetAsync(linkId.ToObjectId(), token);

//            if (link is null)
//                return (false, "The link you were looking for, could not be found");

//            // Temporarily mark the link as deleted
//            // Have task delete it at a later time
//            link.IsDeleted = true;
//            link.DeletedByUserId = deletedById.ToObjectId();

//            link.IsActive = false;
//            link.UpdatedByUserId = deletedById.ToObjectId();
//            link.DateUpdated = DateTime.UtcNow;

//            await LinksRepository.UpdateAsync(link, token);

//            // Log

//            return (true, "");
//        }
//        catch (Exception e)
//        {
//            return (false, e.Message);
//        }
//    }

//    public async Task<Result<string[]>> DeleteLinksAsync(string[] linkIds, string deletedById, CancellationToken token = default)
//    {
//        Guard.Against.NullOrEmpty(linkIds);
//        Guard.Against.NullOrWhiteSpace(deletedById);

//        var deletedIds = new List<string>();

//        foreach (var linkId in linkIds)
//        {
//            var rslt = await DeleteAsync(linkId, deletedById, token);
//            if (rslt.IsSuccess)
//            {
//                deletedIds.Add(linkId);
//            }
//        }

//        return Result.Ok(deletedIds.ToArray());
//    }



//    //public async ValueTask<bool> SubmitLinkAsync(SubmitLinkRequest request, CancellationToken token = default)
//    //{
//    //    var link = await LinksRepository.GetLinkByUrlAsync(request.Url, token);

//    //    var userTags = request.UsersTags.Any() ? request.UsersTags.Select(t => LinkTagEntity.CreateForAdmin(t)).ToArray() : Array.Empty<LinkTagEntity>();

//    //    if (link == null)
//    //    {
//    //        //link = LinkEntity.C
//    //    }


//    //    if (!link.TagsCollection.Any())
//    //    {
//    //        link.TagsCollection.AddRange(userTags);
//    //    }
//    //    else
//    //    {
//    //        foreach (var tag in link.TagsCollection)
//    //        {
//    //            var linkTag = link.TagsCollection.FindLinksAsync(t => t.Name == tag.Name);

//    //            if (linkTag != null)
//    //            {
//    //                linkTag.Count++;
//    //            }
//    //            else
//    //            {
//    //                link.TagsCollection.Add(tag);
//    //            }
//    //        }
//    //    }

//    //    // Associate link with user


//    //    return true;
//    //}
//}