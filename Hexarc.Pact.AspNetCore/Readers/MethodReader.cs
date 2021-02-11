using System;
using System.Linq;
using System.Reflection;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Protocol.TypeReferences;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Hexarc.Pact.AspNetCore.Readers
{
    public sealed class MethodReader
    {
        private TypeChecker TypeChecker { get; }

        private TypeReferenceReader TypeReferenceReader { get; }

        public MethodReader(TypeChecker typeChecker, TypeReferenceReader typeReferenceReader) =>
            (this.TypeChecker, this.TypeReferenceReader) = (typeChecker, typeReferenceReader);

        public Method Read(MethodInfo methodInfo, HttpMethodAttribute methodAttribute, RouteAttribute routeAttribute) =>
            new(methodInfo.Name,
                this.ReadPath(routeAttribute),
                this.ReadHttpMethod(methodAttribute),
                this.ReadReturnType(methodInfo.ReturnType),
                this.ReadMethodParameters(methodInfo.GetParameters()));

        private String ReadPath(RouteAttribute routeAttribute) =>
            routeAttribute.Template.StartsWith("/") ? routeAttribute.Template : $"/{routeAttribute.Template}";

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
