using System;

namespace Hexarc.Rpc.Protocol.TypeReferences
{
    public sealed class PrimitiveTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Primitive;

        public Guid TypeId { get; }

        public PrimitiveTypeReference(Guid typeId) =>
            this.TypeId = typeId;
    }
}
