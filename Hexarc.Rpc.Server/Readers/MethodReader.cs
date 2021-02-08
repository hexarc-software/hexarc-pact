using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Hexarc.Rpc.Protocol.Api;
using Hexarc.Rpc.Protocol.TypeReferences;
using Hexarc.Rpc.Server.Internals;

namespace Hexarc.Rpc.Server.Readers
{
    public sealed class MethodReader
    {
        private TypeChecker TypeChecker { get; }

        private TypeReferenceReader TypeReferenceReader { get; }

        public MethodReader(TypeChecker typeChecker, TypeReferenceReader typeReferenceReader) =>
            (this.TypeChecker, this.TypeReferenceReader) = (typeChecker, typeReferenceReader);

        public Method Read(MethodInfo methodInfo, HttpMethodAttribute methodAttribute) =>
            new(methodInfo.Name, methodAttribute.Template,
                this.ReadHttpMethod(methodAttribute),
                this.ReadReturnType(methodInfo.ReturnType),
                this.ReadMethodParameters(methodInfo.GetParameters()));

        private TypeReference ReadReturnType(Type returnType) =>
            this.TypeChecker.IsTaskType(returnType)
                ? this.TypeReferenceReader.Read(returnType)
                : new TaskTypeReference(default, this.TypeReferenceReader.Read(returnType));

        private MethodParameter[] ReadMethodParameters(ParameterInfo[] parameterInfos) =>
            parameterInfos.Select(this.ReadMethodParameter).ToArray();

        private MethodParameter ReadMethodParameter(ParameterInfo parameterInfo) =>
            new(this.TypeReferenceReader.Read(parameterInfo.ParameterType), parameterInfo.Name!);

        private HttpMethod ReadHttpMethod(HttpMethodAttribute methodAttribute) => methodAttribute switch
        {
            HttpGetAttribute => HttpMethod.Get,
            HttpPostAttribute => HttpMethod.Post,
            _ => throw new NotSupportedException($"No supported attribute {methodAttribute}")
        };
    }
}
