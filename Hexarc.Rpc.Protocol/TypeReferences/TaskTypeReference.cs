using System;

namespace Hexarc.Rpc.Protocol.TypeReferences
{
    public sealed class TaskTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Nullable;

        public Guid TypeId { get; }

        public TypeReference ResultType { get; }

        public TaskTypeReference(Guid typeId, TypeReference resultType) =>
            (this.TypeId, this.ResultType) = (typeId, resultType);
    }
}
