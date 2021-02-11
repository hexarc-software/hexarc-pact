using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Hexarc.Rpc.Server.Readers;

namespace Hexarc.Rpc.Server.Middlewares
{
    public sealed class HexarcRpcMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly HexarcRpcOptions _options;

        private readonly TemplateMatcher _requestMatcher;

        public HexarcRpcMiddleware(RequestDelegate next, HexarcRpcOptions options)
        {
            this._next = next;
            this._options = options;
            this._requestMatcher = new TemplateMatcher(TemplateParser.Parse(options.Route), new RouteValueDictionary());
        }

        public async Task Invoke(HttpContext httpContext, SchemaReader schemaReader)
        {
            if (!this.IsRpcSchemaRequested(httpContext.Request))
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
        private Boolean IsRpcSchemaRequested(HttpRequest request) =>
            this._requestMatcher.TryMatch(request.Path, new RouteValueDictionary());
    }
}
