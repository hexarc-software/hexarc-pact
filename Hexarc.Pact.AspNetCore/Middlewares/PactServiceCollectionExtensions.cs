using Microsoft.Extensions.DependencyInjection;
using Hexarc.Pact.Protocol.TypeProviders;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Readers;

namespace Hexarc.Pact.AspNetCore.Middlewares
{
    public static class PactServiceCollectionExtensions
    {
        public static IServiceCollection AddPactGeneration(this IServiceCollection services)
        {
            services.AddScoped<DistinctTypeQueue>();
            services.AddScoped<PrimitiveTypeProvider>();
            services.AddScoped<DynamicTypeProvider>();
            services.AddScoped<ArrayLikeTypeProvider>();
            services.AddScoped<DictionaryTypeProvider>();
            services.AddScoped<TaskTypeProvider>();
            services.AddScoped<TupleTypeProvider>();
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
