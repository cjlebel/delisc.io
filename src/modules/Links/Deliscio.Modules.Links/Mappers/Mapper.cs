using Deliscio.Modules.Links.Common.Models;
using Deliscio.Modules.Links.Data.Entities;

namespace Deliscio.Modules.Links.Mappers;

public static class Mapper
{
    public static Link? Map(LinkEntity? entity)
    {
        if (entity == null)
            return null;

        var model = new Link(entity.Id, entity.Url, entity.CreatedByUserId);

        return model;
    }

    /// <summary>
    /// Gets an IEnumerable of Link from the collection of LinkEntities.
    /// If any of the converted LinkEntities ends up being null, then they are discarded from the final results.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns>IEnumerable of non-nullable Link</returns>
    public static IEnumerable<Link> Map(IEnumerable<LinkEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<Link>();

        entities = entities as LinkEntity[] ?? entities.ToArray();

        if (!entities.Any())
            return Enumerable.Empty<Link>();

        //var rslts = entities.Select(entity => Map(entity)).Where(link => link != null).ToList();

        var rslts = new List<Link>();

        foreach (var entity in entities)
        {
            var link = Map(entity);

            if (link != null)
                rslts.Add(link);
        }

        return rslts;
    }

    //public static LinkEntity? Map(Link? model)
    //{
    //    if (model == null)
    //        return null;

    //    var entity = new LinkEntity(model.Id, model.Url, model.Title, model.Description, model.Tags);

    //    return entity;
    //}
}
