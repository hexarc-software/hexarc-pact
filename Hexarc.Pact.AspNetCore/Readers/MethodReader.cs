using System;
using System.Linq;
using System.Reflection;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Models;
using Hexarc.Pact.AspNetCore.Extensions;

namespace Hexarc.Pact.AspNetCore.Readers
{
    public sealed class MethodReader
    {
        private TypeChecker TypeChecker { get; }

        private TypeReferenceReader TypeReferenceReader { get; }

        public MethodReader(TypeChecker typeChecker, TypeReferenceReader typeReferenceReader) =>
            (this.TypeChecker, this.TypeReferenceReader) = (typeChecker, typeReferenceReader);

        public Method Read(MethodCandidate methodCandidate) =>
            new(methodCandidate.MethodInfo.Name,
                this.ReadPath(methodCandidate.RouteAttribute!),
                this.ReadHttpMethod(methodCandidate.HttpMethodAttribute!),
                this.ReadMethodResult(methodCandidate.MethodInfo.ReturnType, methodCandidate.IsNullableReferenceResult),
                this.ReadMethodParameters(methodCandidate.MethodInfo.GetParameters()));

        private String ReadPath(RouteAttribute routeAttribute) =>
            routeAttribute.Template.StartsWith("/") ? routeAttribute.Template : $"/{routeAttribute.Template}";

        private MethodResult ReadMethodResult(Type returnType, Boolean isNullableReferenceResult) =>
            this.TryNullifyMethodResult(this.ReadMethodResult(returnType), isNullableReferenceResult);

        private MethodResult TryNullifyMethodResult(MethodResult result, Boolean isNullableReferenceResult) =>
            isNullableReferenceResult
                ? new MethodResult(
                    new TaskTypeReference(result.Type.TypeId, new NullableTypeReference(result.Type.ResultType, true)),
                    result.IsReference)
                : result;

        private MethodResult ReadMethodResult(Type returnType) =>
            this.TypeChecker.IsTaskType(returnType)
                ? new MethodResult((TaskTypeReference)this.TypeReferenceReader.Read(returnType), returnType.GetGenericArguments().First().IsReferenceSemantic())
                : new MethodResult(new TaskTypeReference(default, this.TypeReferenceReader.Read(returnType)), returnType.IsReferenceSemantic());

        private MethodParameter[] ReadMethodParameters(ParameterInfo[] parameterInfos) =>
            parameterInfos.Select(this.ReadMethodParameter).ToArray();

        private MethodParameter ReadMethodParameter(ParameterInfo parameterInfo) =>
            new(this.TypeReferenceReader.Read(parameterInfo.ParameterType), parameterInfo.Name!);

        private HttpMethod ReadHttpMethod(HttpMethodAttribute methodAttribute) => methodAttribute switch
        {
            HttpGetAttribute => HttpMethod.Get,
            HttpPostAttribute => HttpMethod.Post,
            _ => throw new NotSupportedException($"Not supported attribute {methodAttribute}")
        };
    }
}
