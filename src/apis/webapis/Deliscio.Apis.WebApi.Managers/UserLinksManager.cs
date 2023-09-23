using Deliscio.Apis.WebApi.Common.Interfaces;
using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.UserLinks.Common.Models;

namespace Deliscio.Apis.WebApi.Managers;

public sealed class UserLinksManager : IUserLinksManager
{
    /// <summary>
    /// Merges the User's Links with the original links, using the User's version of it's details if they exist.
    /// </summary>
    /// <param name="userLinks">A collection of links that belong to the user</param>
    /// <param name="links">A collection of the original links</param>
    /// <returns></returns>
    public IEnumerable<UserLink> MergeLinks(IEnumerable<UserLink> userLinks, IEnumerable<Link> links)
    {
        // I drew a blank here coming up with names for these.
        var items1 = userLinks.ToList();
        var items2 = links.ToList();

        foreach (var item1 in items1)
        {
            var item2 = items2.Find(l => l.Id == item1.Id);

            if (string.IsNullOrWhiteSpace(item2?.Title))
                continue;

            // If the user's link has information, then use its version, if not, then use the original's
            item1.Title = !string.IsNullOrWhiteSpace(item1.Title) ? item1.Title : item2.Title;
            item1.Description = (!string.IsNullOrWhiteSpace(item1.Description) ? item1.Description : item2.Description) ?? string.Empty;
        }

        return items1;
    }
}