using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    public sealed class DynamicTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Dynamic;

        public Guid TypeId { get; }

        public DynamicTypeReference(Guid typeId) =>
            this.TypeId = typeId;
    }
}
