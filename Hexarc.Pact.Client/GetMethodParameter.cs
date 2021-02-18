using System;

namespace Hexarc.Pact.Client
{
    public sealed class GetMethodParameter
    {
        public String Name { get; }

        public Object Value { get; }

        public String QueryStringKeyValue => $"{this.Name}={this.Value}";

        public GetMethodParameter(String name, Object value) =>
            (this.Name, this.Value) = (name, value);
    }
}
