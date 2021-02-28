using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a literal type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class LiteralTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the LiteralTypeReference kind.
        /// </summary>
        public override String Kind { get; } = TypeReferenceKind.Literal;

        /// <summary>
        /// Gets the literal name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Creates an instance of the LiteralTypeReference class.
        /// </summary>
        /// <param name="name">The literal name.</param>
        public LiteralTypeReference(String name) =>
            this.Name = name;
    }
}
