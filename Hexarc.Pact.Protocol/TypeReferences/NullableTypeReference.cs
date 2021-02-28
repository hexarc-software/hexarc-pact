using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a nullable type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class NullableTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Nullable;

        public TypeReference UnderlyingType { get; }

        public NullableTypeReference(TypeReference underlyingType) =>
            this.UnderlyingType = underlyingType;
    }
}
