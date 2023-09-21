namespace Deliscio.Modules.UserLinks.Models;

public class UserLinkTag
{
    public string Name { get; set; }
    public int Count { get; set; }

    public decimal Weight { get; set; }

    public UserLinkTag(string name, int count, decimal weight)
    {
        Name = name;
        Count = count;
        Weight = weight;
    }

    public static UserLinkTag Create(string name)
    {
        return new UserLinkTag(name, 1, 0);
    }
}