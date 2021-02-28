using System;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes a structure type that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class StructType : ObjectType
    {
        /// <summary>
        /// Gets the StructType kind.
        /// </summary>
        public override String Kind { get; } = TypeKind.Struct;

        /// <summary>
        /// Creates an instance of StructType class.
        /// </summary>
        /// <param name="id">The unique type id</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        /// <param name="genericParameters">The struct generic parameters.</param>
        /// <param name="properties">The struct properties.</param>
        public StructType(Guid id, String? @namespace, String name, String[]? genericParameters, ObjectProperty[] properties) :
            base(id, @namespace, name, false, genericParameters, properties) { }
    }
}
