using System;
using System.Collections.Generic;

namespace CSP
{
    public static class Extensions
    {
        public static T Find<T>(this IReadOnlyList<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i])) return list[i];
            }
            throw new KeyNotFoundException();
        }
    }
}
