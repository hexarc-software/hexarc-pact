using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using HttpMethod = Hexarc.Pact.Protocol.Api.HttpMethod;

namespace Hexarc.Pact.AspNetCore.Readers;

public sealed class MethodReader
{
    private TypeChecker TypeChecker { get; }

    private TypeReferenceReader TypeReferenceReader { get; }

    private NullabilityInfoContext NullabilityInfoContext { get; }

    public MethodReader(
        TypeChecker typeChecker,
        TypeReferenceReader typeReferenceReader,
        NullabilityInfoContext nullabilityInfoContext)
    {
        this.TypeChecker = typeChecker;
        this.TypeReferenceReader = typeReferenceReader;
        this.NullabilityInfoContext = nullabilityInfoContext;
    }

    public Method Read(MethodCandidate methodCandidate, NamingConvention? namingConvention) =>
        new(methodCandidate.MethodInfo.Name.ToConventionalString(namingConvention),
            this.ReadPath(methodCandidate.HttpMethodAttribute!, methodCandidate.RouteAttribute),
            this.ReadHttpMethod(methodCandidate.HttpMethodAttribute!),
            this.ReadMethodReturnType(methodCandidate.MethodInfo.ReturnParameter, namingConvention),
            this.ReadMethodParameters(methodCandidate.MethodInfo.GetParameters(), namingConvention));

    private String ReadPath(HttpMethodAttribute methodAttribute, RouteAttribute? routeAttribute)
    {
        var template = methodAttribute.Template ?? routeAttribute?.Template;
        if (template is null) throw new NullReferenceException("Could not extract method path");
        return template.StartsWith("/") ? template : $"/{template}";
    }

    private TaskTypeReference ReadMethodReturnType(ParameterInfo returnType, NamingConvention? namingConvention)
    {
        if (returnType.ParameterType == typeof(void)) return new TaskTypeReference();

        var contextualType = new ContextualType(this.NullabilityInfoContext.Create(returnType), returnType);
        return returnType switch
        {
            { } when this.TypeChecker.IsTaskType(returnType.ParameterType) =>
                (TaskTypeReference) this.TypeReferenceReader.Read(contextualType, namingConvention),
            _ => new TaskTypeReference(default, this.TypeReferenceReader.Read(contextualType, namingConvention))
        };
    }

    private MethodParameter[] ReadMethodParameters(ParameterInfo[] parameterInfos, NamingConvention? namingConvention) =>
        parameterInfos.Select(x => this.ReadMethodParameter(x, namingConvention)).ToArray();

    private MethodParameter ReadMethodParameter(ParameterInfo parameterInfo, NamingConvention? namingConvention) =>
        new(this.TypeReferenceReader.Read(new ContextualType(this.NullabilityInfoContext.Create(parameterInfo), parameterInfo), namingConvention),
            parameterInfo.Name ?? throw new InvalidOperationException());

    private HttpMethod ReadHttpMethod(HttpMethodAttribute methodAttribute) => methodAttribute switch
    {
        HttpGetAttribute => HttpMethod.Get,
        HttpPostAttribute => HttpMethod.Post,
        _ => throw new NotSupportedException($"Not supported attribute {methodAttribute}")
    };
}
