namespace Deliscio.Modules.Links.Common.Models;

public class LinkTag
{
    public string Name { get; set; }
    public int Count { get; set; }

    public decimal Weight { get; set; }

    // Needed for deserialization
    public LinkTag() { }

    public LinkTag(string name, int count)
    {
        Name = name;
        Count = count;
    }

    public LinkTag(string name, int count, decimal weight)
    {
        Name = name;
        Count = count;
        Weight = weight;
    }

    public static LinkTag Create(string name)
    {
        return new LinkTag(name, 1, 0);
    }
}