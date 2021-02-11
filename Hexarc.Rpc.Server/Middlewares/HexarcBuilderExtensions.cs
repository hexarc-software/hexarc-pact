using Microsoft.AspNetCore.Builder;

namespace Hexarc.Rpc.Server.Middlewares
{
    public static class HexarcBuilderExtensions
    {
        public static IApplicationBuilder UseHexarcRpc(this IApplicationBuilder app) =>
            app.UseMiddleware<HexarcRpcMiddleware>(new HexarcRpcOptions());

        public static IApplicationBuilder UseHexarcRpc(this IApplicationBuilder app, HexarcRpcOptions options) =>
            app.UseMiddleware<HexarcRpcMiddleware>(options);
    }
}
