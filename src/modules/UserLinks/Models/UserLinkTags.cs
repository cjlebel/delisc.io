using System.Collections;

namespace Deliscio.Modules.UserLinks.Models;

public class UserLinkTags : IEnumerable<UserLinkTag>
{
    private readonly List<UserLinkTag> _tags;

    public void Add(UserLinkTag tag)
    {
        _tags.Add(tag);

        RecalculateWeight();
    }


    public IEnumerator<UserLinkTag> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public UserLinkTags() : this(Array.Empty<UserLinkTag>()) { }

    public UserLinkTags(IEnumerable tags)
    {
        _tags = tags as List<UserLinkTag> ?? new List<UserLinkTag>();
    }

    private void RecalculateWeight()
    {
        if (_tags is [])
            return;

        var totalCounts = _tags.Sum(t => t.Count);

        if (totalCounts <= 0)
            return;

        foreach (var t in _tags)
        {
            t.Weight = (decimal)t.Count / totalCounts;
        }
    }
}