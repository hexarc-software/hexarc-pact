using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Hexarc.Pact.AspNetCore.Readers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Hexarc.Pact.AspNetCore.Middlewares
{
    public sealed class PactMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly PactOptions _options;

        private readonly TemplateMatcher _requestMatcher;

        public PactMiddleware(RequestDelegate next, PactOptions options)
        {
            this._next = next;
            this._options = options;
            this._requestMatcher = new TemplateMatcher(TemplateParser.Parse(options.Route), new RouteValueDictionary());
        }

        public async Task Invoke(HttpContext httpContext, SchemaReader schemaReader)
        {
            if (!this.IsPactSchemaRequested(httpContext.Request))
            {
                await this._next(httpContext);
                return;
            }

            var schema = schemaReader.Read(this._options.AssemblyWithControllers);
            var result = new OkObjectResult(schema);
            var routeData = httpContext.GetRouteData();
            var actionDescriptor = new ActionDescriptor();
            var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);
            await result.ExecuteResultAsync(actionContext);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Boolean IsPactSchemaRequested(HttpRequest request) =>
            this._requestMatcher.TryMatch(request.Path, new RouteValueDictionary());
    }
}
