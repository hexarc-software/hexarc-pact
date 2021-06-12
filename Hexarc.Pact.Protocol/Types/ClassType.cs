using System;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes a class type that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class ClassType : ObjectType
    {
        /// <summary>
        /// Gets the <see cref="ClassType"/> kind.
        /// </summary>
        public override String Kind => TypeKind.Class;

        /// <summary>
        /// Creates an instance of <see cref="ClassType"/> class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        /// <param name="typeParameters">The class type parameters.</param>
        /// <param name="properties">The class properties.</param>
        public ClassType(Guid id, String? @namespace, String name, String[]? typeParameters, ObjectProperty[] properties) :
            base(id, @namespace, name, true, typeParameters, properties) { }
    }
}
