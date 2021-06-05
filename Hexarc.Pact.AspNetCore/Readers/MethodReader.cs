using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Namotion.Reflection;

using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.AspNetCore.Extensions;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Models;

namespace Hexarc.Pact.AspNetCore.Readers
{
    public sealed class MethodReader
    {
        private TypeChecker TypeChecker { get; }

        private TypeReferenceReader TypeReferenceReader { get; }

        public MethodReader(TypeChecker typeChecker, TypeReferenceReader typeReferenceReader) =>
            (this.TypeChecker, this.TypeReferenceReader) = (typeChecker, typeReferenceReader);

        public Method Read(MethodCandidate methodCandidate, NamingConvention? namingConvention) =>
            new(methodCandidate.MethodInfo.Name.ToConventionalString(namingConvention),
                this.ReadPath(methodCandidate.RouteAttribute!),
                this.ReadHttpMethod(methodCandidate.HttpMethodAttribute!),
                this.ReadMethodReturnType(methodCandidate.MethodInfo.ReturnParameter, namingConvention),
                this.ReadMethodParameters(methodCandidate.MethodInfo.GetContextualParameters(), namingConvention));

        private String ReadPath(RouteAttribute routeAttribute) =>
            routeAttribute.Template.StartsWith("/") ? routeAttribute.Template : $"/{routeAttribute.Template}";

        private TaskTypeReference ReadMethodReturnType(ParameterInfo returnType, NamingConvention? namingConvention)
        {
            if (returnType.ParameterType == typeof(void)) return new TaskTypeReference();
            return returnType.ToContextualParameter() switch
            {
                { } x when this.TypeChecker.IsTaskType(x) => (TaskTypeReference) this.TypeReferenceReader.Read(x, namingConvention),
                { } x => new TaskTypeReference(default, this.TypeReferenceReader.Read(x, namingConvention))
            };
        }

        private MethodParameter[] ReadMethodParameters(ContextualParameterInfo[] parameterInfos, NamingConvention? namingConvention) =>
            parameterInfos.Select(x => this.ReadMethodParameter(x, namingConvention)).ToArray();

        private MethodParameter ReadMethodParameter(ContextualParameterInfo parameterInfo, NamingConvention? namingConvention) =>
            new(this.TypeReferenceReader.Read(parameterInfo, namingConvention), parameterInfo.Name!);

        private HttpMethod ReadHttpMethod(HttpMethodAttribute methodAttribute) => methodAttribute switch
        {
            HttpGetAttribute => HttpMethod.Get,
            HttpPostAttribute => HttpMethod.Post,
            _ => throw new NotSupportedException($"Not supported attribute {methodAttribute}")
        };
    }
}
