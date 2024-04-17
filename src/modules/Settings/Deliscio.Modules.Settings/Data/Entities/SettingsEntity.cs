using System.ComponentModel.DataAnnotations.Schema;
using Deliscio.Core.Data.Interfaces;
using Deliscio.Core.Data.Mongo;
using Deliscio.Core.Data.Mongo.Attributes;
using Deliscio.Modules.Settings.Common;
using MongoDB.Bson;

namespace Deliscio.Modules.Settings.Data.Entities;

/// <summary>
/// Represents a link that has been submitted to the central repository.
/// </summary>
/// <seealso cref="MongoEntityBase" />
[Table("Settings")]
[BsonCollection("settings")]
//public sealed class SettingsEntity<out TCategoryName> : MongoEntityObjectIdBase, IIsSoftDeletableBy<ObjectId>
public sealed class SettingsEntity : MongoEntityBase, IIsSoftDeletableBy<ObjectId>
{
    public string Name { get; init; }

    public string Description { get; set; } = "";

    public string Environment { get; set; } = "";

    public ISettings Values { get; set; }

    public int Version { get; internal set; }

    public bool IsPublished { get; set; }

    public ObjectId PublishedById { get; set; } = ObjectId.Empty;

    public DateTimeOffset? PublishedDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DateDeleted { get; set; }

    public ObjectId DeletedById { get; set; } = ObjectId.Empty;

    public SettingsEntity() { }

    public SettingsEntity(ObjectId id, string name, string environment, ISettings values, int version, ObjectId createdById) : base(id, createdById)
    {
        Name = name;
        Environment = environment;
        Values = values;
        Version = version;
    }

    public static SettingsEntity Create<TValues>(string name, string environment, ISettings values, string createdById)
    {
        var typeName = typeof(TValues).Name;

        var entity = new SettingsEntity(ObjectId.GenerateNewId(), name, environment, values, 1, ObjectId.Parse(createdById));

        return entity;
    }
}