namespace Hexarc.Pact.Demo.Api.Middlewares;

using System;
using Microsoft.AspNetCore.Builder;

public static class ApplicationExtensions
{
    public static IApplicationBuilder EnforceSslForForwardedProto(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            if (context.Request.Headers.ContainsKey("X-Original-Proto") && !context.Request.IsHttps)
            {
                var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : String.Empty;
                var https = $"https://{context.Request.Host}{context.Request.Path}{queryString}";
                context.Response.Redirect(https, true);
            }
            else
            {
                await next();
            }
        });
        return app;
    }
}
