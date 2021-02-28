using System;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes a class type that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class ClassType : ObjectType
    {
        /// <summary>
        /// Gets the ClassType kind.
        /// </summary>
        public override String Kind { get; } = TypeKind.Class;

        /// <summary>
        /// Creates an instance of ClassType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        /// <param name="genericParameters">The class generic parameters.</param>
        /// <param name="properties">The class properties.</param>
        public ClassType(Guid id, String? @namespace, String name, String[]? genericParameters, ObjectProperty[] properties) :
            base(id, @namespace, name, true, genericParameters, properties) { }
    }
}
