using System;
using Hexarc.Pact.Protocol.TypeReferences;

namespace Hexarc.Pact.Protocol.Api
{
    public sealed class Method
    {
        public String Name { get; }

        public String Path { get; }

        public HttpMethod HttpMethod { get; }

        public TaskTypeReference ReturnType { get; }

        public MethodParameter[] Parameters { get; }

        public Method(
            String name,
            String path,
            HttpMethod httpMethod,
            TaskTypeReference returnType,
            MethodParameter[] parameters)
        {
            this.Name = name;
            this.Path = path;
            this.HttpMethod = httpMethod;
            this.ReturnType = returnType;
            this.Parameters = parameters;
        }
    }
}
