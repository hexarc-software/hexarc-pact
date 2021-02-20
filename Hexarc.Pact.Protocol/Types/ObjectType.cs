using System;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Contains the common logic for all object-like types.
    /// </summary>
    public abstract class ObjectType : DistinctType
    {
        /// <summary>
        /// Gets the generic type parameters.
        /// </summary>
        public String[]? GenericParameters { get; }

        /// <summary>
        /// Gets the type properties.
        /// </summary>
        public ObjectProperty[] Properties { get; }

        /// <summary>
        /// Creates an instance of the ObjectType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        /// <param name="isReference">The type reference semantic marker.</param>
        /// <param name="genericParameters">The generic parameters.</param>
        /// <param name="properties">The type own properties.</param>
        protected ObjectType(
            Guid id, String? @namespace, String name, Boolean isReference,
            String[]? genericParameters, ObjectProperty[] properties
        ) : base(id, @namespace, name, isReference) =>
            (this.GenericParameters, this.Properties) = (genericParameters, properties);
    }
}
