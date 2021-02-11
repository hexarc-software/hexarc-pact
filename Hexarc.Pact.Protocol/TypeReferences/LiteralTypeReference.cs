using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    public sealed class LiteralTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Literal;

        public String Name { get; }

        public LiteralTypeReference(String name) =>
            this.Name = name;
    }
}
