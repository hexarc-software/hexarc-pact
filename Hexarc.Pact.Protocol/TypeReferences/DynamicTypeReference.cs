using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a dynamic type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DynamicTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Dynamic;

        public Guid TypeId { get; }

        public DynamicTypeReference(Guid typeId) =>
            this.TypeId = typeId;
    }
}
