using Deliscio.Core.Data.Interfaces;
using Deliscio.Core.Data.Mongo;
using MongoDB.Bson;

namespace Deliscio.Modules.Links.Infrastructure.Data.Entities;

// Note: LinkTagEntity and UserLinkTagEntity are identical.
//       Not sure how to share them nicely, while also keeping the domains separate.
//       I could create a shared library in Modules, but, then things will be spread out more so than they are now.
public class LinkTagEntity : MongoEntityBase, IIsSoftDeletableBy<ObjectId>
{
    private string _name = string.Empty;

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DateDeleted { get; set; }

    public ObjectId DeletedById { get; set; }


    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value.Replace('/', '-').Trim().ToLowerInvariant();
        }
    }

    public int Count { get; set; }

    public decimal Weight { get; set; }

    public LinkTagEntity(string name, int count = 1, decimal weight = 0)
    {
        Name = name.Replace('/', '-').ToLowerInvariant().Trim();
        Count = count;
        Weight = weight;
    }

    //public LinkTagEntity(string name)
    //{
    //    Name = name;
    //}

    //public static LinkTagEntity CreateForAdmin(string name)
    //{
    //    return new LinkTagEntity(name);
    //}
}