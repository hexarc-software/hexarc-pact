using System;
using System.Collections.Generic;
using System.Linq;

namespace Hexarc.Pact.Protocol.TypeProviders
{
    /// <summary>
    /// The stock tuple type provider used in the Hexarc Pact protocol.
    /// </summary>
    public sealed class TupleTypeProvider
    {
        private Type TupleOfOne { get; } = typeof(ValueTuple<>);

        private Type TupleOfTwo { get; } = typeof(ValueTuple<,>);

        private Type TupleOfThree { get; } = typeof(ValueTuple<,,>);

        private Type TupleOfFour { get; } = typeof(ValueTuple<,,,>);

        private Type TupleOfFive { get; } = typeof(ValueTuple<,,,,>);

        private Type TupleOfSix { get; } = typeof(ValueTuple<,,,,,>);

        private Type TupleOfSeven { get; } = typeof(ValueTuple<,,,,,,>);

        private Type TupleOfEight { get; } = typeof(ValueTuple<,,,,,,,>);

        /// <summary>
        /// Gets the registered array-like type ids.
        /// </summary>
        public IReadOnlySet<Guid> TypeIds { get; }

        /// <summary>
        /// Creates an instance of the TupleTypeProvider class.
        /// </summary>
        public TupleTypeProvider() =>
            this.TypeIds = this.Enumerate()
                .Select(x => x.GUID)
                .ToHashSet();

        /// <summary>
        /// Enumerates the registered array-like types.
        /// </summary>
        /// <returns>The registered array-like type collection.</returns>
        public IEnumerable<Type> Enumerate()
        {
            yield return this.TupleOfOne;
            yield return this.TupleOfTwo;
            yield return this.TupleOfThree;
            yield return this.TupleOfFour;
            yield return this.TupleOfFive;
            yield return this.TupleOfSix;
            yield return this.TupleOfSeven;
            yield return this.TupleOfEight;
        }
    }
}
