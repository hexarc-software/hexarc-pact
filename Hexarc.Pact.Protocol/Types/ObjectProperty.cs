using System;
using Hexarc.Pact.Protocol.TypeReferences;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes a property of an object-like type.
    /// </summary>
    public sealed class ObjectProperty
    {
        /// <summary>
        /// Gets the property type by it's reference.
        /// </summary>
        public TypeReference Type { get; }

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Creates an instance of the ObjectProperty class.
        /// </summary>
        /// <param name="type">The type reference to qualify the property's type</param>
        /// <param name="name">The property name.</param>
        public ObjectProperty(TypeReference type, String name) =>
            (this.Type, this.Name) = (type, name);
    }
}
