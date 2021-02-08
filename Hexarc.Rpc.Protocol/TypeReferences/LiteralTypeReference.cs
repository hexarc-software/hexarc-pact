using System;

namespace Hexarc.Rpc.Protocol.TypeReferences
{
    public sealed class LiteralTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Literal;

        public String Name { get; }

        public LiteralTypeReference(String name) =>
            this.Name = name;
    }
}
