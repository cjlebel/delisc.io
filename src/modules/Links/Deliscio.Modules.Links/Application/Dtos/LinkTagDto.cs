using Deliscio.Modules.Links.Domain.Links;
using Deliscio.Modules.Links.Domain.LinkTags;

namespace Deliscio.Modules.Links.Application.Dtos;

public sealed record LinkTagDto
{
    public string Name { get; set; }
    public int Count { get; set; }

    public decimal Weight { get; set; }

    // Needed for deserialization
    public LinkTagDto(string name, int count, decimal weight)
    {
        Name = name;
        Count = count;
        Weight = weight;
    }

    //public LinkTag(string name, int count)
    //{
    //    Name = name;
    //    Count = count;
    //}

    //public LinkTag(string name, int count, float weight)
    //{
    //    Name = name.Trim();
    //    Count = count;
    //    Weight = float.Round(weight, 6);
    //}

    //public static LinkTag CreateForAdmin(string name)
    //{
    //    return new LinkTag(name, 1, 0);
    //}

    public static explicit operator LinkTagDto(LinkTag tag)
    {
        if(tag is null)
            throw new ArgumentNullException(nameof(tag));

        return new LinkTagDto(tag.Name, tag.Count, tag.Weight);
    }
}