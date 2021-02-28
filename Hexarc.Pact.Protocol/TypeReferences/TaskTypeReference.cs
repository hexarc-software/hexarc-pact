using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a task type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class TaskTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Task;

        public Guid? TypeId { get; }

        public TypeReference ResultType { get; }

        public TaskTypeReference(Guid? typeId, TypeReference resultType) =>
            (this.TypeId, this.ResultType) = (typeId, resultType);
    }
}
