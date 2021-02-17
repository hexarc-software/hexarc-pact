using System.Collections.Generic;

namespace Hexarc.Pact.Tool.Internals
{
    public static class EnumerableFactory
    {
        public static IEnumerable<T> FromOne<T>(T element)
        {
            yield return element;
        }
    }
}
