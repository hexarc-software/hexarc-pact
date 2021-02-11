using Microsoft.Extensions.DependencyInjection;
using Hexarc.Rpc.Protocol.TypeProviders;
using Hexarc.Rpc.Server.Internals;
using Hexarc.Rpc.Server.Readers;

namespace Hexarc.Rpc.Server.Middlewares
{
    public static class HexarcRpcServiceCollectionExtensions
    {
        public static IServiceCollection AddHexarcRpcGeneration(this IServiceCollection services)
        {
            services.AddScoped<DistinctTypeQueue>();
            services.AddScoped<PrimitiveTypeProvider>();
            services.AddScoped<ArrayLikeTypeProvider>();
            services.AddScoped<DictionaryTypeProvider>();
            services.AddScoped<TaskTypeProvider>();
            services.AddScoped<TypeChecker>();
            services.AddScoped<TypeReferenceReader>();
            services.AddScoped<DistinctTypeReader>();
            services.AddScoped<MethodReader>();
            services.AddScoped<MethodReader>();
            services.AddScoped<ControllerReader>();
            services.AddTransient<SchemaReader>();

            return services;
        }
    }
}
