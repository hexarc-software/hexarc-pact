using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    public sealed class NullableTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Nullable;

        public TypeReference UnderlyingType { get; }

        public NullableTypeReference(TypeReference underlyingType) =>
            this.UnderlyingType = underlyingType;
    }
}
