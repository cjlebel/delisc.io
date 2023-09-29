using Deliscio.Modules.UserLinks.Common.Models;
using Deliscio.Modules.UserLinks.Data.Entities;

namespace Deliscio.Modules.UserLinks.Mappers;

public static class Mapper
{
    #region Models => Entities
    internal static UserLinkEntity? Map(UserLink? link)
    {
        if (link == null)
            return null;

        var tags = Map(link.Tags)?.ToArray() ?? Array.Empty<UserLinkTagEntity>();

        var entity = new UserLinkEntity(Guid.Parse(link.Id), Guid.Parse(link.UserId), Guid.Parse(link.LinkId), link.DateCreated, link.DateUpdated, tags);

        return entity;
    }

    internal static IEnumerable<UserLinkEntity> Map(IEnumerable<UserLink> links)
    {
        var linksArr = links?.ToArray() ?? Array.Empty<UserLink>();

        if (!linksArr.Any())
            return Enumerable.Empty<UserLinkEntity>();

        var rslts = new List<UserLinkEntity>();

        foreach (var link in linksArr)
        {
            var entity = Map(link);

            if (entity != null)
                rslts.Add(entity);
        }

        return rslts;
    }

    internal static UserLinkTagEntity? Map(UserLinkTag? tag)
    {
        if (tag is null)
            return null;

        var entity = new UserLinkTagEntity(tag.Name, tag.Count);

        return entity;
    }

    internal static IEnumerable<UserLinkTagEntity> Map(IEnumerable<UserLinkTag>? tags)
    {
        if (tags == null)
            return Enumerable.Empty<UserLinkTagEntity>();

        var tagsArr = tags.ToArray();

        if (!tagsArr.Any())
            return Enumerable.Empty<UserLinkTagEntity>();

        var rslts = new List<UserLinkTagEntity>();

        foreach (var tag in tagsArr)
        {
            var entity = Map(tag);

            if (entity != null)
                rslts.Add(entity);
        }

        return rslts;
    }
    #endregion

    #region Entities => Models
    internal static UserLink? Map(UserLinkEntity? entity)
    {
        if (entity == null)
            return null;

        var tags = Map(entity.Tags)?.ToArray() ?? Array.Empty<UserLinkTag>();

        var model = new UserLink(entity.Id.ToString(), entity.LinkId.ToString(), entity.UserId.ToString(), entity.Title, entity.DateCreated, entity.DateUpdated, tags);

        return model;
    }

    internal static IEnumerable<UserLink> Map(IEnumerable<UserLinkEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<UserLink>();

        var entitiesArr = entities.ToArray();

        if (!entitiesArr.Any())
            return Enumerable.Empty<UserLink>();

        var rslts = new List<UserLink>();

        foreach (var entity in entitiesArr)
        {
            var link = Map(entity);

            if (link != null)
                rslts.Add(link);
        }

        return rslts;
    }

    internal static UserLinkTag? Map(UserLinkTagEntity? entity)
    {
        if (entity == null)
            return null;

        var model = new UserLinkTag(entity.Name, entity.Count, entity.Weight);

        return model;
    }

    internal static IEnumerable<UserLinkTag> Map(IEnumerable<UserLinkTagEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<UserLinkTag>();

        entities = entities.ToList();

        if (!entities.Any())
            return Enumerable.Empty<UserLinkTag>();

        var rslts = new List<UserLinkTag>();

        foreach (var entity in entities)
        {
            var tag = Map(entity);

            if (tag != null)
                rslts.Add(tag);
        }

        return rslts;
    }
    #endregion

}