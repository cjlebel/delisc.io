using Deliscio.Core.Data.Interfaces;
using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Infrastructure.Data.Entities;
using FluentResults;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Infrastructure.Data;

public interface ILinksRepository : IRepository<LinkEntity, ObjectId>
{
    #region - Links -

    Task<Result<(IReadOnlyList<Domain.Links.Link> Results, int TotalPages, int TotalCount)>> FindLinksAsync(
        string term, string[] tags, string domain, 
        int pageNo, int pageSize, int offset = 0, 
        bool? isActive = default, bool? isFlagged = default, bool? isDeleted = false,
        CancellationToken token = default);

    Task<Result<Link>> GetLinkByIdAsync(string linkId, CancellationToken token = default);

    Task<Result<Link>> GetLinkByUrlAsync(string url, CancellationToken token = default);

    Task<Result<IEnumerable<Link>>> GetLinksByUserAsync(string userId, CancellationToken token = default);

    //Task<(IEnumerable<LinkEntity> Results, int TotalPages, int Count)> GetLinksByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    //Task<Result> DeleteLinkAsync(string linkId, string userId, CancellationToken token = default);

    //Task<Result<string[]>> DeleteLinksAsync(List<Link> links, CancellationToken token = default);

    Task<Result> UpdateLinkAsync(Link link, string updatedByUserId, CancellationToken token = default);

    #endregion

    #region - TagsCollection -

    ///// <summary>
    ///// Adds a tag to the Link with the id.<br />
    ///// If the tag already exists, then its count will be incremented by 1.
    ///// </summary>
    ///// <param name="linkId">The id of the Link to update.</param>
    ///// <param name="tag">The tag in which its count will be incremented.</param>
    ///// <param name="token">The token.</param>
    ///// <returns>Task</returns>
    //Task AddTagAsync(Guid linkId, string tag, CancellationToken token);

    Task<Result<IEnumerable<Domain.LinkTags.LinkTag>>> GetTagsAsync(string linkId, CancellationToken token);

    Task<IEnumerable<LinkTagEntity>> GetRelatedTagsAsync(string[] tags, int count, CancellationToken token = default);

    Task<IEnumerable<LinkTagEntity>> GetRelatedTagsByDomainAsync(string domain, int count, CancellationToken token = default);

    ///// <summary>
    ///// Removes an occurrence of the tag from the Tag collection that belongs to Link with the id.<br />
    ///// </summary>
    ///// <param name="linkId">The id of the Link to update.</param>
    ///// <param name="tag">The tag in which its count will be incremented.</param>
    ///// <param name="token">The token.</param>
    ///// <returns>Task</returns>
    //Task RemoveTagAsync(Guid linkId, string tag, CancellationToken token);
    #endregion

}