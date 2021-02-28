using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a task type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class TaskTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the TaskTypeReference kind.
        /// </summary>
        public override String Kind { get; } = TypeReferenceKind.Task;

        /// <summary>
        /// Gets the unique task type id.
        /// </summary>
        public Guid? TypeId { get; }

        /// <summary>
        /// Gets the reference to the task result type.
        /// </summary>
        public TypeReference ResultType { get; }

        /// <summary>
        /// Creates an instance of the TaskTypeReference class.
        /// </summary>
        /// <param name="typeId">The unique task type id.</param>
        /// <param name="resultType">The reference to the task result type.</param>
        public TaskTypeReference(Guid? typeId, TypeReference resultType) =>
            (this.TypeId, this.ResultType) = (typeId, resultType);
    }
}
