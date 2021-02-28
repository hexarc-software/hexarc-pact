using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    // TODO: Maybe rename to GenericParameter.
    /// <summary>
    /// Describes a generic type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class GenericTypeReference : TypeReference
    {
        /// <summary>
        /// Gets the GenericTypeReference kind.
        /// </summary>
        public override String Kind { get; } = TypeReferenceKind.Generic;

        /// <summary>
        /// Gets the generic parameter name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Creates an instance of the GenericTypeReference class.
        /// </summary>
        /// <param name="name">The name of the generic parameter.</param>
        public GenericTypeReference(String name) =>
            this.Name = name;
    }
}
