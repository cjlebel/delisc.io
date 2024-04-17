using Deliscio.Modules.Settings.Data.Entities;

namespace Deliscio.Modules.Settings.Mappers;

public static class Mapper
{
    public static Common.Settings? Map(SettingsEntity? entity)
    {
        if (entity is null)
            return default;

        var settings = new Common.Settings
        {
            Id = entity.Id.ToString(),
            Name = entity.Name,
            Description = entity.Description,
            Environment = entity.Environment,
            Values = entity.Values,
            Version = entity.Version,
            DateCreated = entity.DateCreated,
            DateUpdated = entity.DateUpdated,
            IsDeleted = entity.IsDeleted,
            DateDeleted = entity.DateDeleted,
            DeletedById = entity.DeletedById.ToString(),
        };

        return settings;
    }

    public static SettingsEntity Map(Common.Settings Settings)
    {
        var entity = new SettingsEntity
        {
            Id = new MongoDB.Bson.ObjectId(Settings.Id),
            Description = Settings.Description,
            Environment = Settings.Environment,
            Name = Settings.Name,
            Values = Settings.Values,
            Version = Settings.Version,
            DateCreated = Settings.DateCreated,
            DateUpdated = Settings.DateUpdated,
            IsDeleted = Settings.IsDeleted,
            DateDeleted = Settings.DateDeleted,
            DeletedById = new MongoDB.Bson.ObjectId(Settings.DeletedById),
        };

        return entity;
    }
}