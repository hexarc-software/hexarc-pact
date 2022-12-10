using Microsoft.AspNetCore.Http;

namespace Hexarc.Pact.AspNetCore.Middlewares;

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
        var nullabilityInfoContext = new NullabilityInfoContext();
        var typeReferenceReader = new TypeReferenceReader(typeChecker, distinctTypeQueue);
        var distinctTypeReader = new DistinctTypeReader(typeChecker, typeReferenceReader, nullabilityInfoContext);
        var methodReader = new MethodReader(typeChecker, typeReferenceReader, nullabilityInfoContext);
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
            ? request.Query["scope"].ToHashSet<String>()
            : default;
}
