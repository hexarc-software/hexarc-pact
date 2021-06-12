using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Hexarc.Pact.AspNetCore.Extensions;
using Hexarc.Pact.AspNetCore.Models;
using Hexarc.Serialization.Union;
using Hexarc.Pact.AspNetCore.Readers;

namespace Hexarc.Pact.AspNetCore.Middlewares
{
    public sealed class PactSchemaService
    {
        private readonly PactOptions _options;

        private readonly JsonSerializerOptions _jsonOptions;

        public PactSchemaService(PactOptions options)
        {
            this._options = options;
            this._jsonOptions = new JsonSerializerOptions
            {
                Converters = { new UnionConverterFactory() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
        }

        public async Task HandleRequest(HttpContext httpContext)
        {
            var namingConvention = this.ExtractNamingConvention(httpContext.Request);
            var scopes = this.ExtractScopes(httpContext.Request);
            var schemaReader = httpContext.RequestServices.GetRequiredService<SchemaReader>();
            var assembly = this._options.AssemblyWithControllers;
            var schema = scopes switch
            {
                not null => schemaReader.Read(assembly.GetPactScopedTypes(scopes), namingConvention),
                _ => schemaReader.Read(assembly, namingConvention)
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
