using Deliscio.Core.Data.Interfaces;

namespace Deliscio.Modules.SubmittedLinks.Abstractions.Interfaces;

public interface ISubmittedLinksRepository : IRepository<SubmittedLinkEntity>
{


    #region - Tags -
    /// <summary>
    /// Adds a tag to the SubmittedLink with the id.<br />
    /// If the tag already exists, then its count will be incremented by 1.
    /// </summary>
    /// <param name="id">The id of the SubmittedLink to update.</param>
    /// <param name="tag">The tag in which its count will be incremented.</param>
    /// <param name="token">The token.</param>
    /// <returns>Task</returns>
    Task AddTag(Guid id, string tag, CancellationToken token);

    /// <summary>
    /// Gets the tags that are associated with the Submitted link with the id.
    /// </summary>
    /// <param name="id">The id of the SubmittedLink to retrieve the tags from.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    Task<IReadOnlyDictionary<string, int>> GetTags(Guid id, CancellationToken token);

    /// <summary>
    /// Removes an occurrence of the tag from the Tag collection that belongs to SubmittedLink with the id.<br />
    /// </summary>
    /// <param name="id">The id of the SubmittedLink to update.</param>
    /// <param name="tag">The tag in which its count will be incremented.</param>
    /// <param name="token">The token.</param>
    /// <returns>Task</returns>
    Task RemoveTag(Guid id, string tag, CancellationToken token);
    #endregion

}