using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    public sealed class DictionaryTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Dictionary;

        public Guid TypeId { get; }

        public TypeReference KeyType { get; }

        public TypeReference ValueType { get; }

        public DictionaryTypeReference(Guid typeId, TypeReference keyType, TypeReference valueType) =>
            (this.TypeId, this.KeyType, this.ValueType) = (typeId, keyType, valueType);

        public DictionaryTypeReference(Guid typeId, TypeReference[] keyAndValueTypes) :
            this(typeId, keyAndValueTypes[0], keyAndValueTypes[1]) { }
    }
}
