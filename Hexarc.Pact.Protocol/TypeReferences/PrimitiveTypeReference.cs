using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a primitive type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class PrimitiveTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the <see cref="PrimitiveTypeReference"/> kind.
        /// </summary>
        public override String Kind => TypeReferenceKind.Primitive;

        /// <summary>
        /// Gets the unique type id.
        /// </summary>
        public Guid TypeId { get; }

        /// <summary>
        /// Creates an instance of the <see cref="PrimitiveTypeReference"/> class.
        /// </summary>
        /// <param name="typeId">The unique type id.</param>
        public PrimitiveTypeReference(Guid typeId) =>
            this.TypeId = typeId;
    }
}
