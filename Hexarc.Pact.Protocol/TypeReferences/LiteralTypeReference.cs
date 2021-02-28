using System;

namespace Hexarc.Pact.Protocol.TypeReferences
{
    /// <summary>
    /// Describes a literal type reference that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class LiteralTypeReference : TypeReference
    {
        public override String Kind { get; } = TypeReferenceKind.Literal;

        public String Name { get; }

        public LiteralTypeReference(String name) =>
            this.Name = name;
    }
}
