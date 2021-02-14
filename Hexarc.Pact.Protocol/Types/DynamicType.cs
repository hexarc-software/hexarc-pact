using System;
using Hexarc.Pact.Protocol.Internals;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes a dynamic type that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class DynamicType : Type
    {
        public override String Kind { get; } = TypeKind.Dynamic;

        /// <summary>
        /// Creates an instance of the DynamicType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        public DynamicType(Guid id, String? @namespace, String name) :
            base(id, @namespace, name) { }

        /// <summary>
        /// Creates an instance of the DynamicType class.
        /// </summary>
        /// <param name="type">The native .NET type to create the class instance from.</param>
        public DynamicType(System.Type type) :
            this(type.GUID, type.Namespace, type.NameWithoutGenericArity()) { }
    }
}
