using System;
using System.Collections.Generic;
using System.Linq;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    /// <summary>
    /// The stock array-like type provider used in the Hexarc Pact protocol.
    /// </summary>
    public sealed class ArrayLikeTypeProvider
    {
        private ArrayLikeType EnumerableOfT { get; } = new(typeof(IEnumerable<>));

        private ArrayLikeType ListOfT { get; } = new(typeof(List<>));

        private ArrayLikeType HashSetOf { get; } = new(typeof(HashSet<>));

        public IReadOnlySet<Guid> TypeIds { get; }

        public ArrayLikeTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        public IEnumerable<ArrayLikeType> Enumerate()
        {
            yield return this.EnumerableOfT;
            yield return this.ListOfT;
            yield return this.HashSetOf;
        }
    }
}
