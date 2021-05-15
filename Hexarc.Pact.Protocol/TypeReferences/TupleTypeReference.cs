using System;
using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a tuple type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class TupleTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the TupleTypeReference kind.
        /// </summary>
        public override String Kind => TypeReferenceKind.Tuple;

        /// <summary>
        /// Gets the tuple elements.
        /// </summary>
        public TupleElement[] Elements { get; }

        /// <summary>
        /// Creates an instance of the TupleTypeReference class.
        /// </summary>
        /// <param name="elements">The tuple elements.</param>
        public TupleTypeReference(TupleElement[] elements) =>
            this.Elements = elements;
    }
}
