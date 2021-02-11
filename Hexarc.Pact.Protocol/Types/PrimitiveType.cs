using System;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes a primitive type that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class PrimitiveType : Type
    {
        public override String Kind { get; } = TypeKind.Primitive;

        /// <summary>
        /// Creates an instance of the PrimitiveType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        public PrimitiveType(Guid id, String? @namespace, String name) :
            base(id, @namespace, name) { }

        /// <summary>
        /// Creates an instance of the PrimitiveType class.
        /// </summary>
        /// <param name="type">The native .NET type to create the class instance from.</param>
        public PrimitiveType(System.Type type) :
            this(type.GUID, type.Namespace, type.Name) { }
    }
}
