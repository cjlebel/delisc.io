using Deliscio.Modules.BackLog.Data.Entities;
using Deliscio.Modules.BackLog.Models;

namespace Deliscio.Modules.Backlog.Mappers;

internal static class Mappers
{
    /// <summary>
    /// Maps a single model to its entity version.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    internal static BacklogItemEntity? Map(BacklogItem? model)
    {
        if (model == null)
            return null;

        //var entity = new BackLinkEntity(model.Title, model.Url, model.CreatedById, model.IsProcessed, model.Tags)
        var entity = new BacklogItemEntity(model.Id, model.Title, model.Url, model.CreatedById)
        {
            DateCreated = model.DateCreated,
        };

        return entity;
    }

    /// <summary>
    /// Maps a collection of models to their entity version.
    /// </summary>
    /// <param name="models">The models to map to the entity representation.</param>
    /// <returns></returns>
    internal static IEnumerable<BacklogItemEntity> Map(IEnumerable<BacklogItem>? models)
    {
        if (models == null)
            return Enumerable.Empty<BacklogItemEntity>();

        return models.Select(model => Map(model)).Where(entity => entity != null).ToList()!;
    }

    /// <summary>
    /// Maps the entity to its model version.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">entity</exception>
    internal static BacklogItem? Map(BacklogItemEntity? entity)
    {
        if (entity == null)
            return null;

        var model = new BacklogItem(entity.Id, entity.Title, entity.Url, entity.CreatedById, entity.DateCreated, entity.DateUpdated, entity.IsProcessed);

        return model;
    }

    /// <summary>
    /// Maps a collection of entities to their model version.
    /// </summary>
    /// <param name="entities">The entities to map to the model representation.</param>
    /// <returns></returns>
    internal static IEnumerable<BacklogItem> Map(IEnumerable<BacklogItemEntity>? entities)
    {
        if (entities == null)
            return Enumerable.Empty<BacklogItem>();

        return entities.Select(entity => Map(entity)).Where(entity => entity != null).ToList()!;
    }
}