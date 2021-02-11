using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    public sealed class GenericTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Generic;

        public String Name { get; }

        public GenericTypeReference(String name) =>
            this.Name = name;
    }
}
