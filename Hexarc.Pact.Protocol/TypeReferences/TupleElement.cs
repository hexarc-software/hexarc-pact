using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a tuple element object.
    /// </summary>
    public sealed class TupleElement
    {
        /// <summary>
        /// Gets the tuple element type by it's reference.
        /// </summary>
        public TypeReference Type { get; }

        /// <summary>
        /// Gets the tuple element name.
        /// </summary>
        public String? Name { get; }

        /// <summary>
        /// Creates an instance of <see cref="TupleElement"/> type.
        /// </summary>
        /// <param name="type">The tuple element type.</param>
        /// <param name="name">The tuple element name if presented.</param>
        public TupleElement(TypeReference type, String? name) =>
            (this.Type, this.Name) = (type, name);
    }
}
