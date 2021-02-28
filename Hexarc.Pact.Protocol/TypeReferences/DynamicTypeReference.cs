using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a dynamic type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DynamicTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the DynamicTypeReference kind.
        /// </summary>
        public override String Kind { get; } = TypeReferenceKind.Dynamic;

        /// <summary>
        /// Gets the unique dynamic type id.
        /// </summary>
        public Guid TypeId { get; }

        /// <summary>
        /// Creates an instance of the DynamicTypeReference class.
        /// </summary>
        /// <param name="typeId">The unique dynamic type id.</param>
        public DynamicTypeReference(Guid typeId) =>
            this.TypeId = typeId;
    }
}
