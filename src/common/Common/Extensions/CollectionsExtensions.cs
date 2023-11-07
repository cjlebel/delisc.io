namespace Deliscio.Common.Extensions;

public static class CollectionsExtensions
{
    public static T[] GetArrayOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        var array = enumerable?.ToArray() ?? Array.Empty<T>();

        return array;
    }

    public static List<T> GetListOrEmpty<T>(this IEnumerable<T>? enumerable)
    {
        var list = enumerable?.ToList() ?? new List<T>();

        return list;
    }
}