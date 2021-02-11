using Microsoft.Extensions.DependencyInjection;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Readers;
using Hexarc.Pact.Protocol.TypeProviders;

namespace Hexarc.Pact.AspNetCore.Middlewares
{
    public static class PactServiceCollectionExtensions
    {
        public static IServiceCollection AddPactGeneration(this IServiceCollection services)
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
