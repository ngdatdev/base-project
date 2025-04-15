using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Helper;

/// <summary>
///     Collection Util.
/// </summary>
public static class CollectionUtil
{
    public static IEnumerable<T> DistinctBy<T, TKey>(
        this IEnumerable<T> items,
        Func<T, TKey> keySelector
    )
    {
        var seenKeys = new HashSet<TKey>();
        foreach (var item in items)
        {
            if (seenKeys.Add(keySelector(item)))
                yield return item;
        }
    }

    public static IEnumerable<T> RemoveNulls<T>(this IEnumerable<T> source) =>
        source.Where(x => x != null);

    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int size) =>
        source
            .Select((x, i) => new { i, x })
            .GroupBy(x => x.i / size)
            .Select(g => g.Select(x => x.x));
}
