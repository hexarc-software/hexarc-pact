using System;
using System.Text.Json.Serialization;
using Hexarc.Pact.Protocol.Extensions;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes a dynamic type that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DynamicType : Type
    {
        /// <summary>
        /// Gets the DynamicType kind.
        /// </summary>
        public override String Kind => TypeKind.Dynamic;

        /// <summary>
        /// Creates an instance of the DynamicType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        /// <param name="isReference">The type reference semantic marker.</param>
        [JsonConstructor]
        public DynamicType(Guid id, String? @namespace, String name, Boolean isReference) :
            base(id, @namespace, name, isReference) { }

        /// <summary>
        /// Creates an instance of the DynamicType class.
        /// </summary>
        /// <param name="type">The native .NET type to create the class instance from.</param>
        public DynamicType(System.Type type) :
            this(type.GUID, type.Namespace, type.NameWithoutGenericArity(), type.IsReference()) { }
    }
}
