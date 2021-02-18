using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    public sealed class NullableTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Nullable;

        public TypeReference UnderlyingType { get; }

        public Boolean IsReference { get; }

        public NullableTypeReference(TypeReference underlyingType, Boolean isReference) =>
            (this.UnderlyingType, this.IsReference) = (underlyingType, isReference);
    }
}
