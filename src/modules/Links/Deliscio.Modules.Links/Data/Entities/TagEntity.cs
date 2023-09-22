using Deliscio.Core.Data.Mongo;

namespace Deliscio.Modules.Links.Data.Entities;

public class LinkTagEntity : MongoEntityBase
{
    public string Name { get; set; }

    public int Count { get; set; }

    public decimal Weight { get; set; }

    public LinkTagEntity(string name, int count = 1, decimal weight = 0)
    {
        Name = name;
        Count = count;
        Weight = weight;
    }

    public static LinkTagEntity Create(string name)
    {
        return new LinkTagEntity(name);
    }
}