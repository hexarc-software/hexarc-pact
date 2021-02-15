using System;
using System.Collections.Generic;

namespace Hexarc.Pact.Tool.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Separate<T>(this IEnumerable<T> enumerable, Int32 count, T separator)
        {
            var current = 0;
            foreach (var x in enumerable)
            {
                if (current < count - 1)
                {
                    yield return x;
                    yield return separator;
                }
                else
                {
                    yield return x;
                }
                current++;
            }
        }
    }
}
