using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a distinct type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DistinctTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the <see cref="DistinctTypeReference"/> kind.
        /// </summary>
        public override String Kind => TypeReferenceKind.Distinct;

        /// <summary>
        /// Gets the distinct type of the reference.
        /// </summary>
        public Guid TypeId { get; }

        /// <summary>
        /// Gets the distinct type arguments.
        /// </summary>
        public TypeReference[]? TypeArguments { get; }

        /// <summary>
        /// Creates an instance of the <see cref="DistinctTypeReference"/> class.
        /// </summary>
        /// <param name="typeId">The unique distinct type id.</param>
        /// <param name="typeArguments">The distinct type arguments.</param>
        public DistinctTypeReference(Guid typeId, TypeReference[]? typeArguments = default) =>
            (this.TypeId, this.TypeArguments) = (typeId, typeArguments);
    }
}
