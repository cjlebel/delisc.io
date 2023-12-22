using Deliscio.Core.Data.Mongo;

namespace Deliscio.Modules.Links.Data.Entities;

// Note: LinkTagEntity and UserLinkTagEntity are identical.
//       Not sure how to share them nicely, while also keeping the domains separate.
//       I could create a shared library in Modules, but, then things will be spread out more so than they are now.
public class LinkTagEntity : MongoEntityBase
{
    private string _name = string.Empty;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value.ToLowerInvariant();
        }
    }

    public int Count { get; set; }

    public float Weight { get; set; }

    public LinkTagEntity(string name, int count = 1, float weight = 0)
    {
        Name = name.Replace('/', '-');
        Count = count;
        Weight = weight;
    }

    public static LinkTagEntity Create(string name)
    {
        return new LinkTagEntity(name);
    }
}