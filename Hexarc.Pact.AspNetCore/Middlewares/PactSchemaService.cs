using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Hexarc.Pact.AspNetCore.Extensions;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Models;
using Hexarc.Serialization.Union;
using Hexarc.Pact.AspNetCore.Readers;
using Hexarc.Pact.Protocol.TypeProviders;

namespace Hexarc.Pact.AspNetCore.Middlewares
{
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
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
            var primitiveTypeProvider = new PrimitiveTypeProvider();
            var dynamicTypeProvider = new DynamicTypeProvider();
            var arrayLikeTypeProvider = new ArrayLikeTypeProvider();
            var dictionaryTypeProvider = new DictionaryTypeProvider();
            var taskTypeProvider = new TaskTypeProvider();
            var tupleTypeProvider = new TupleTypeProvider();
            var typeChecker = new TypeChecker(
                primitiveTypeProvider,
                dynamicTypeProvider,
                arrayLikeTypeProvider,
                dictionaryTypeProvider,
                taskTypeProvider,
                tupleTypeProvider);
            var distinctTypeQueue = new DistinctTypeQueue();
            var typeReferenceReader = new TypeReferenceReader(typeChecker, distinctTypeQueue);
            var distinctTypeReader = new DistinctTypeReader(typeChecker, typeReferenceReader);
            var methodReader = new MethodReader(typeChecker, typeReferenceReader);
            var controllerReader = new ControllerReader(methodReader);
            this._schemaReader = new SchemaReader(
                distinctTypeQueue,
                distinctTypeReader,
                controllerReader,
                primitiveTypeProvider,
                dynamicTypeProvider,
                arrayLikeTypeProvider,
                dictionaryTypeProvider,
                taskTypeProvider);
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
}
