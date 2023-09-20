namespace Deliscio.Modules.Links.Models;

public class Tag
{
    public string Name { get; set; }
    public int Count { get; set; }

    public decimal Weight { get; set; }

    public Tag(string name, int count, decimal weight)
    {
        Name = name;
        Count = count;
        Weight = weight;
    }

    public static Tag Create(string name)
    {
        return new Tag(name, 1, 0);
    }
}