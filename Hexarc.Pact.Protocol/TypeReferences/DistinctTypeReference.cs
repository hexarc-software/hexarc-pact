using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a distinct type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DistinctTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Distinct;

        public Guid TypeId { get; }

        public TypeReference[]? GenericArguments { get; }

        public DistinctTypeReference(Guid typeId, TypeReference[]? genericArguments = default) =>
            (this.TypeId, this.GenericArguments) = (typeId, genericArguments);
    }
}
