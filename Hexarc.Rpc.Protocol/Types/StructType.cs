using System;

namespace Hexarc.Rpc.Protocol.Types
{
    /// <summary>
    /// Describes a structure type that can be provided by the Hexarc RPC protocol.
    /// </summary>
    public sealed class StructType : ObjectType
    {
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
            base(id, @namespace, name, genericParameters, properties) { }
    }
}
