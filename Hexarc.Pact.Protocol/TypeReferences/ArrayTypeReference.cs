using System;
using System.Text.Json.Serialization;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes an array type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class ArrayTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Array;

        /// <summary>
        /// Gets the array-like type id when the type is not presented by the system array type.
        /// </summary>
        public Guid? ArrayLikeTypeId { get; }

        /// <summary>
        /// Gets the array element type.
        /// </summary>
        public TypeReference ElementType { get; }

        /// <summary>
        /// Creates an instance of the ArrayTypeReference class.
        /// </summary>
        /// <param name="arrayLikeTypeId">The array-like type id if presented.</param>
        /// <param name="elementType">The array element type.</param>
        [JsonConstructor]
        public ArrayTypeReference(Guid? arrayLikeTypeId, TypeReference elementType) =>
            (this.ArrayLikeTypeId, this.ElementType) = (arrayLikeTypeId, elementType);

        /// <summary>
        /// Creates an instance of the ArrayTypeReference class.
        /// </summary>
        /// <param name="elementType">The array element type.</param>
        public ArrayTypeReference(TypeReference elementType) :
            this(default, elementType) { }
    }
}
