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

        /// <summary>
        /// Gets the registered array-like type ids.
        /// </summary>
        public IReadOnlySet<Guid> TypeIds { get; }

        /// <summary>
        /// Creates an instance of the ArrayLikeTypeProvider class.
        /// </summary>
        public ArrayLikeTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.Id)
                .ToHashSet();

        /// <summary>
        /// Enumerates the registered array-like types.
        /// </summary>
        /// <returns>The registered array-like type collection.</returns>
        public IEnumerable<ArrayLikeType> Enumerate()
        {
            yield return this.EnumerableOfT;
            yield return this.ListOfT;
            yield return this.HashSetOf;
        }
    }
}
