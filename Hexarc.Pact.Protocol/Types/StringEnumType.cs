using System;

namespace Hexarc.Pact.Protocol.Types
{
    public sealed class StringEnumType : DistinctType
    {
        public override String Kind { get; } = TypeKind.StringEnum;

        public String[] Members { get; }

        public StringEnumType(Guid id, String? @namespace, String name, String[] members) :
            base(id, @namespace, name, false) => this.Members = members;
    }
}
