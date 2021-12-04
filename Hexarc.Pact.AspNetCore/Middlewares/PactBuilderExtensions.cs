using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hexarc.Pact.AspNetCore.Middlewares;

public static class PactBuilderExtensions
{
    public static IApplicationBuilder UsePact(this IApplicationBuilder app) =>
        app.UsePact("/pact/schema");

    public static IApplicationBuilder UsePact(this IApplicationBuilder app, String schemaPath) =>
        app.Map(schemaPath, HandlePact);

    private static void HandlePact(IApplicationBuilder app) =>
        app.Run(async context =>
        {
            var schemaService = context.RequestServices.GetRequiredService<PactSchemaService>();
            await schemaService.HandleRequest(context);
        });
}
