using System.Collections.Generic;

namespace System.Linq;

public static class Linq
{
    public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }
        if (predicate == null)
        {
            throw new ArgumentNullException("predicate");
        }
        foreach (TSource item in source)
        {
            if (predicate(item))
            {
                return item;
            }
        }
        return defaultValue;
    }
}
