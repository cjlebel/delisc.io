using Deliscio.Core.Data.Interfaces;
using Deliscio.Modules.Links.Data.Entities;

namespace Deliscio.Modules.Links.Common.Interfaces;

public interface ILinksRepository : IRepository<LinkEntity>
{
    #region - Links -

    Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> FindAsync(string term, string tags, string domain, int pageNo, int pageSize,
        int skip = 0, bool? isActive = default, bool? isFlagged = default, bool? isDeleted = false,
        CancellationToken token = default);

    Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> FindAsync(string term, string[] tags, string domain, int pageNo, int pageSize,
        int skip = 0, bool? isActive = default, bool? isFlagged = default, bool? isDeleted = false,
        CancellationToken token = default);

    Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> GetLinksByDomainAsync(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> GetLinksByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<LinkEntity?> GetLinkByUrlAsync(string url, CancellationToken token = default);

    #endregion

    #region - Tags -
    /// <summary>
    /// Adds a tag to the Link with the id.<br />
    /// If the tag already exists, then its count will be incremented by 1.
    /// </summary>
    /// <param name="id">The id of the Link to update.</param>
    /// <param name="tag">The tag in which its count will be incremented.</param>
    /// <param name="token">The token.</param>
    /// <returns>Task</returns>
    Task AddTagAsync(Guid linkId, string tag, CancellationToken token);

    Task<IEnumerable<LinkTagEntity>> GetRelatedTagsAsync(string[] tags, int count, CancellationToken token = default);

    Task<IEnumerable<LinkTagEntity>> GetRelatedTagsByDomainAsync(string domain, int count, CancellationToken token = default);

    /// <summary>
    /// Removes an occurrence of the tag from the Tag collection that belongs to Link with the id.<br />
    /// </summary>
    /// <param name="id">The id of the Link to update.</param>
    /// <param name="tag">The tag in which its count will be incremented.</param>
    /// <param name="token">The token.</param>
    /// <returns>Task</returns>
    Task RemoveTagAsync(Guid linkId, string tag, CancellationToken token);
    #endregion

}