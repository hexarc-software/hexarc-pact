using System;
using System.Text.Json.Serialization;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a dictionary type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DictionaryTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Dictionary;

        public Guid TypeId { get; }

        public TypeReference KeyType { get; }

        public TypeReference ValueType { get; }

        [JsonConstructor]
        public DictionaryTypeReference(Guid typeId, TypeReference keyType, TypeReference valueType) =>
            (this.TypeId, this.KeyType, this.ValueType) = (typeId, keyType, valueType);

        public DictionaryTypeReference(Guid typeId, TypeReference[] keyAndValueTypes) :
            this(typeId, keyAndValueTypes[0], keyAndValueTypes[1]) { }
    }
}
