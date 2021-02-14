using System;
using System.Text.Json.Serialization;
using Hexarc.Pact.Protocol.Internals;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes an array-like type that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class ArrayLikeType : Type
    {
        public override String Kind { get; } = TypeKind.ArrayLike;

        /// <summary>
        /// Creates an instance of the ArrayLikeType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        [JsonConstructor]
        public ArrayLikeType(Guid id, String? @namespace, String name) :
            base(id, @namespace, name) { }

        /// <summary>
        /// Creates an instance of the ArrayLikeType class.
        /// </summary>
        /// <param name="type">The native .NET type to create the class instance from.</param>
        public ArrayLikeType(System.Type type) :
            this(type.GUID, type.Namespace, type.NameWithoutGenericArity()) { }
    }
}
