namespace Deliscio.Core.Data.Interfaces;

/// <summary>
/// For entities that are soft deletable by a specific user.
/// Requires a property called DeletedById.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IIsSoftDeletableBy<T> : IIsSoftDeletable
{
    T DeletedById { get; set; }
}