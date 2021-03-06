using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    /// <summary>
    /// The stock array-like type provider used in the Hexarc Pact protocol.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public sealed class ArrayLikeTypeProvider
    {
        private ArrayLikeType IEnumerableOfT { get; } = new(typeof(IEnumerable<>));

        private ArrayLikeType ICollectionOfT { get; } = new(typeof(ICollection<>));

        private ArrayLikeType IReadOnlyCollection { get; } = new(typeof(IReadOnlyCollection<>));

        private ArrayLikeType ListOfT { get; } = new(typeof(List<>));

        private ArrayLikeType IListOfT { get; } = new(typeof(IList<>));

        private ArrayLikeType IReadOnlyListOfT { get; } = new(typeof(IReadOnlyList<>));

        private ArrayLikeType HashSetOf { get; } = new(typeof(HashSet<>));

        private ArrayLikeType ISetOf { get; } = new(typeof(ISet<>));

        private ArrayLikeType IReadOnlySetOfT { get; } = new(typeof(IReadOnlySet<>));

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
            yield return this.IEnumerableOfT;
            yield return this.ICollectionOfT;
            yield return this.IReadOnlyCollection;
            yield return this.ListOfT;
            yield return this.IListOfT;
            yield return this.IReadOnlyListOfT;
            yield return this.HashSetOf;
            yield return this.ISetOf;
            yield return this.IReadOnlySetOfT;
        }
    }
}
