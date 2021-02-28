using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a generic type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class GenericTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Generic;

        public String Name { get; }

        public GenericTypeReference(String name) =>
            this.Name = name;
    }
}
