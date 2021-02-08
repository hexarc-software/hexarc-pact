using System;

namespace Hexarc.Rpc.Protocol.TypeReferences
{
    public sealed class DistinctTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Distinct;

        public Guid TypeId { get; }

        public TypeReference[]? GenericArguments { get; }

        public DistinctTypeReference(Guid typeId, TypeReference[]? genericArguments = default) =>
            (this.TypeId, this.GenericArguments) = (typeId, genericArguments);
    }
}
