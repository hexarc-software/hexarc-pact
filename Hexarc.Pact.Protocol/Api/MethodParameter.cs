using System;
using Hexarc.Pact.Protocol.TypeReferences;

namespace Hexarc.Pact.Protocol.Api
{
    public sealed class MethodParameter
    {
        public TypeReference Type { get; }

        public String Name { get; }

        public MethodParameter(TypeReference type, String name) =>
            (this.Type, this.Name) = (type, name);
    }
}
