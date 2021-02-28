using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a primitive type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class PrimitiveTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Primitive;

        public Guid TypeId { get; }

        public PrimitiveTypeReference(Guid typeId) =>
            this.TypeId = typeId;
    }
}
