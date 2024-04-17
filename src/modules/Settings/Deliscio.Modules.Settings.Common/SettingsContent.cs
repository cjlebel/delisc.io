namespace Deliscio.Modules.Settings.Common;

/// <summary>
/// A wrapper for the actual settings that are saved.
/// </summary>
public record Settings
{
    public string Id { get; set; } = "";

    public string Name { get; init; } = "";

    public string Description { get; set; } = "";

    public string Environment { get; set; } = "";

    public ISettings Values { get; init; }

    public int Version { get; init; }

    public DateTimeOffset DateCreated { get; init; }

    public string CreatedById { get; init; }

    public DateTimeOffset? DateUpdated { get; set; }

    public string UpdatedById { get; set; } = string.Empty;

    public bool IsPublished { get; set; }

    public DateTimeOffset? PublishedDate { get; set; }

    public string PublishedById { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DateDeleted { get; set; }

    public string DeletedById { get; set; } = string.Empty;

    public Settings() { }

    public Settings(string id, string name, string description, string environment, ISettings values, int version, DateTimeOffset dateCreated, string createdById, DateTimeOffset? dateUpdated, string updatedById, bool isDeleted, DateTimeOffset? dateDeleted, string deletedById)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(id));

        Id = id;
        Name = name;
        Description = description;
        Environment = environment;
        Values = values;
        Version = version;
        DateCreated = dateCreated;
        CreatedById = createdById;
        DateUpdated = dateUpdated;
        UpdatedById = updatedById;
        IsDeleted = isDeleted;
        DateDeleted = dateDeleted;
        DeletedById = deletedById;
    }

    public static Settings Create(string name, string description, string environment, ISettings values)
    {
        var typeName = values.GetType().Name; // typeof(values).Name;

        var entity = new Settings()
        {
            Description = description,
            Name = name,
            Values = values,
        };

        return entity;
    }
}