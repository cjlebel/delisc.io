namespace Deliscio.Modules.Links.Common.Models;

public sealed record LinkTag
{
    public string Name { get; set; }
    public int Count { get; set; }

    public float Weight { get; set; }

    // Needed for deserialization
    public LinkTag() { }

    public LinkTag(string name, int count)
    {
        Name = name;
        Count = count;
    }

    public LinkTag(string name, int count, float weight)
    {
        Name = name;
        Count = count;
        Weight = float.Round(weight, 6);
    }

    public static LinkTag Create(string name)
    {
        return new LinkTag(name, 1, 0);
    }
}