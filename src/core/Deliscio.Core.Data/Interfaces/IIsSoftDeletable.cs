namespace Deliscio.Core.Data.Interfaces;

/// <summary>
/// For entities that are soft deletable.
/// This is useful for entities that are flagged as deleted, and are not hard deleted from the database.
/// Requires a boolean property called IsDeleted to glag if deleted, and a DateDeleted of type DateTimeOffset.
/// </summary>
public interface IIsSoftDeletable
{
    bool IsDeleted { get; set; }

    DateTimeOffset DateDeleted { get; set; }
}