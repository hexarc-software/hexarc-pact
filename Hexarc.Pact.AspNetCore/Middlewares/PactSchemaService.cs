namespace Hexarc.Pact.AspNetCore.Middlewares;

using Microsoft.AspNetCore.Http;
using Hexarc.Serialization.Union;
using Hexarc.Pact.AspNetCore.Extensions;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Models;
using Hexarc.Pact.AspNetCore.Readers;

public sealed class PactSchemaService
{
    private readonly PactOptions _options;

    private readonly JsonSerializerOptions _jsonOptions;

    private readonly SchemaReader _schemaReader;

    public PactSchemaService(PactOptions options)
    {
        this._options = options;
        this._jsonOptions = new JsonSerializerOptions
        {
            Converters = { new UnionConverterFactory() },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var typeProvider = this._options.TypeProvider ?? new TypeProvider();
        var typeChecker = new TypeChecker(typeProvider);
        var distinctTypeQueue = new DistinctTypeQueue();
        var typeReferenceReader = new TypeReferenceReader(typeChecker, distinctTypeQueue);
        var distinctTypeReader = new DistinctTypeReader(typeChecker, typeReferenceReader);
        var methodReader = new MethodReader(typeChecker, typeReferenceReader);
        var controllerReader = new ControllerReader(methodReader);
        this._schemaReader = new SchemaReader(distinctTypeQueue, distinctTypeReader, controllerReader, typeProvider);
    }

    public async Task HandleRequest(HttpContext httpContext)
    {
        var namingConvention = this.ExtractNamingConvention(httpContext.Request);
        var scopes = this.ExtractScopes(httpContext.Request);
        var assembly = this._options.AssemblyWithControllers;
        var schema = scopes switch
        {
            not null => this._schemaReader.Read(assembly.GetPactScopedTypes(scopes), namingConvention),
            _ => this._schemaReader.Read(assembly, namingConvention)
        };
        await httpContext.Response.WriteAsJsonAsync(schema, this._jsonOptions);
    }

    private NamingConvention? ExtractNamingConvention(HttpRequest request) =>
        request.Query.ContainsKey("namingConvention")
            ? EnumExtensions.Parse<NamingConvention>(request.Query["namingConvention"])
            : default;

    private HashSet<String>? ExtractScopes(HttpRequest request) =>
        request.Query.ContainsKey("scope")
            ? request.Query["scope"].ToHashSet()
            : default;
}
