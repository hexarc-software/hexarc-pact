using System;
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

            var schemaReader = httpContext.RequestServices.GetRequiredService<SchemaReader>();
            var namingConvention = this.ExtractNamingConvention(httpContext.Request);
            var schema = schemaReader.Read(this._options.AssemblyWithControllers, namingConvention);
            await httpContext.Response.WriteAsJsonAsync(schema, this._jsonOptions);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean IsPactSchemaRequested(HttpRequest request) =>
            this._requestMatcher.TryMatch(request.Path, new RouteValueDictionary());

        private NamingConvention? ExtractNamingConvention(HttpRequest request) =>
            EnumExtensions.Parse<NamingConvention>(request.Query["namingConvention"]);
    }
}
