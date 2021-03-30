using System;
using System.Linq;

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
                this.ReadMethodReturnType(methodCandidate.MethodInfo.ReturnParameter.ToContextualParameter()),
                this.ReadMethodParameters(methodCandidate.MethodInfo.GetContextualParameters()));

        private String ReadPath(RouteAttribute routeAttribute) =>
            routeAttribute.Template.StartsWith("/") ? routeAttribute.Template : $"/{routeAttribute.Template}";

        private TaskTypeReference ReadMethodReturnType(ContextualType returnType) =>
            this.TypeChecker.IsTaskType(returnType)
                ? (TaskTypeReference)this.TypeReferenceReader.Read(returnType)
                : new TaskTypeReference(default, this.TypeReferenceReader.Read(returnType));

        private MethodParameter[] ReadMethodParameters(ContextualParameterInfo[] parameterInfos) =>
            parameterInfos.Select(this.ReadMethodParameter).ToArray();

        private MethodParameter ReadMethodParameter(ContextualParameterInfo parameterInfo) =>
            new(this.TypeReferenceReader.Read(parameterInfo), parameterInfo.Name!);

        private HttpMethod ReadHttpMethod(HttpMethodAttribute methodAttribute) => methodAttribute switch
        {
            HttpGetAttribute => HttpMethod.Get,
            HttpPostAttribute => HttpMethod.Post,
            _ => throw new NotSupportedException($"Not supported attribute {methodAttribute}")
        };
    }
}
