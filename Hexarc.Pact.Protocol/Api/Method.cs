using System;

namespace Hexarc.Pact.Protocol.Api
{
    public sealed class Method
    {
        public String Name { get; }

        public String Path { get; }

        public HttpMethod HttpMethod { get; }

        public MethodResult Result { get; }

        public MethodParameter[] Parameters { get; }

        public Method(
            String name,
            String path,
            HttpMethod httpMethod,
            MethodResult result,
            MethodParameter[] parameters)
        {
            this.Name = name;
            this.Path = path;
            this.HttpMethod = httpMethod;
            this.Result = result;
            this.Parameters = parameters;
        }
    }
}
