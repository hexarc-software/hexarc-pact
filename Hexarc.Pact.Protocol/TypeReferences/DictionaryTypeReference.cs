using System;
using System.Text.Json.Serialization;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a dictionary type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DictionaryTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the <see cref="DictionaryTypeReference"/> kind.
        /// </summary>
        public override String Kind => TypeReferenceKind.Dictionary;

        /// <summary>
        /// Gets the unique dictionary type id.
        /// </summary>
        public Guid TypeId { get; }

        /// <summary>
        /// Gets the dictionary key type.
        /// </summary>
        public TypeReference KeyType { get; }

        /// <summary>
        /// Gets the dictionary value type.
        /// </summary>
        public TypeReference ValueType { get; }

        /// <summary>
        /// Creates an instance of the <see cref="DictionaryTypeReference"/> class.
        /// </summary>
        /// <param name="typeId">The unique dictionary type id.</param>
        /// <param name="keyType">The dictionary key type.</param>
        /// <param name="valueType">The dictionary value type.</param>
        [JsonConstructor]
        public DictionaryTypeReference(Guid typeId, TypeReference keyType, TypeReference valueType) =>
            (this.TypeId, this.KeyType, this.ValueType) = (typeId, keyType, valueType);

        /// <summary>
        /// Creates an instance of the <see cref="DictionaryTypeReference"/> type.
        /// </summary>
        /// <param name="typeId">The unique dictionary type id.</param>
        /// <param name="keyAndValueTypes">
        /// The array where the first element is the key type
        /// reference and the second element is the value type reference.
        /// </param>
        public DictionaryTypeReference(Guid typeId, TypeReference[] keyAndValueTypes) :
            this(typeId, keyAndValueTypes[0], keyAndValueTypes[1]) { }
    }
}
