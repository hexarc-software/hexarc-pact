using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a distinct type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DistinctTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the DistinctTypeReference kind.
        /// </summary>
        public override String Kind { get; } = TypeReferenceKind.Distinct;

        /// <summary>
        /// Gets the distinct type of the reference.
        /// </summary>
        public Guid TypeId { get; }

        /// <summary>
        /// Gets the distinct type generic parameters.
        /// </summary>
        public TypeReference[]? GenericArguments { get; }

        /// <summary>
        /// Creates an instance of the DistinctTypeReference class.
        /// </summary>
        /// <param name="typeId">The unique distinct type id.</param>
        /// <param name="genericArguments">The distinct type generic arguments.</param>
        public DistinctTypeReference(Guid typeId, TypeReference[]? genericArguments = default) =>
            (this.TypeId, this.GenericArguments) = (typeId, genericArguments);
    }
}
