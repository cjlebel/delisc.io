namespace Deliscio.Core.Data.Interfaces;

/// <summary>
/// For entities that are soft deletable by a specific user.
/// Requires a property called DeletedById.
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface IIsSoftDeletableBy<TId> : IIsSoftDeletable
{
    TId DeletedById { get; set; }
}