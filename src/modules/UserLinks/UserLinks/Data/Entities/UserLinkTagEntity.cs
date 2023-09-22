using Deliscio.Core.Data.Mongo;

namespace Deliscio.Modules.UserLinks.Data.Entities;

public class UserLinkTagEntity : MongoEntityBase
{
    public string Name { get; set; }

    public int Count { get; set; }

    public decimal Weight { get; set; }

    public UserLinkTagEntity(string name, int count = 1, decimal weight = 0)
    {
        Name = name;
        Count = count;
        Weight = weight;
    }

    public static UserLinkTagEntity Create(string name)
    {
        return new UserLinkTagEntity(name);
    }
}