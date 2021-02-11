using Microsoft.AspNetCore.Builder;

namespace Hexarc.Pact.AspNetCore.Middlewares
{
    public static class PactBuilderExtensions
    {
        public static IApplicationBuilder UsePact(this IApplicationBuilder app) =>
            app.UseMiddleware<PactMiddleware>(new PactOptions());

        public static IApplicationBuilder UsePact(this IApplicationBuilder app, PactOptions options) =>
            app.UseMiddleware<PactMiddleware>(options);
    }
}
