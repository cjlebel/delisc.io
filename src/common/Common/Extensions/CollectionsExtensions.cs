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
        var list = enumerable?.ToList() ?? [];

        return list;
    }

    public static string[] GetArrayOrEmpty(this string s, char separator)
    {
        if (string.IsNullOrWhiteSpace(s.Trim()))
        {
            return Array.Empty<string>();
        }

        var result = !string.IsNullOrWhiteSpace(s) ?
            s.Split(separator).ToArray() :
            Array.Empty<string>();

        return result;
    }
}