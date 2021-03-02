using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;

using Hexarc.Pact.AspNetCore.Extensions;
using Hexarc.Pact.AspNetCore.Models;
using Hexarc.Serialization.Union;
using Hexarc.Pact.AspNetCore.Readers;

namespace Hexarc.Pact.AspNetCore.Middlewares
{
    public sealed class PactMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly PactOptions _options;

        private readonly TemplateMatcher _requestMatcher;

        private readonly JsonSerializerOptions _jsonOptions;

        public PactMiddleware(RequestDelegate next, PactOptions options)
        {
            this._next = next;
            this._options = options;
            this._requestMatcher = new TemplateMatcher(TemplateParser.Parse(options.Route), new RouteValueDictionary());
            this._jsonOptions = new JsonSerializerOptions
            {
                Converters = { new UnionConverterFactory() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!this.IsPactSchemaRequested(httpContext.Request))
            {
                await this._next(httpContext);
                return;
            }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean IsPactSchemaRequested(HttpRequest request) =>
            this._requestMatcher.TryMatch(request.Path, new RouteValueDictionary());

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
