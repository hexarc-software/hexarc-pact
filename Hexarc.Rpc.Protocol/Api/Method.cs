using System;
using Hexarc.Rpc.Protocol.TypeReferences;

namespace Hexarc.Rpc.Protocol.Api
{
    public sealed class Method
    {
        public String Name { get; }

        public String Path { get; }

        public HttpMethod HttpMethod { get; }

        public TypeReference ResultType { get; }

        public MethodParameter[] Parameters { get; }

        public Method(
            String name,
            String path,
            HttpMethod httpMethod,
            TypeReference resultType,
            MethodParameter[] parameters)
        {
            this.Name = name;
            this.Path = path;
            this.HttpMethod = httpMethod;
            this.ResultType = resultType;
            this.Parameters = parameters;
        }

    }
}
