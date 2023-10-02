using Deliscio.Core.Data.Interfaces;
using Deliscio.Modules.Links.Data.Entities;

namespace Deliscio.Modules.Links.Interfaces;

public interface ILinksRepository : IRepository<LinkEntity>
{
    #region - Links -

    Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> GetByDomainAsync(string domain, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<(IEnumerable<LinkEntity> Results, int TotalPages, int TotalCount)> GetByTagsAsync(IEnumerable<string> tags, int pageNo = 1, int pageSize = 25, CancellationToken token = default);

    Task<LinkEntity?> GetByUrlAsync(string url, CancellationToken token = default);

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

    /// <summary>
    /// Gets the tags that are associated with the Submitted link with the id.
    /// </summary>
    /// <param name="id">The id of the Link to retrieve the tags from.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<IEnumerable<LinkTagEntity>> GetTagsAsync(Guid linkId, CancellationToken token);

    Task<IEnumerable<LinkTagEntity>> GetRelatedTagsAsync(string[] tags, int count, CancellationToken token = default);

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