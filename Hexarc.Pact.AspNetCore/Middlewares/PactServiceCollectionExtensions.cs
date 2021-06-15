using System;
using Microsoft.Extensions.DependencyInjection;

namespace Hexarc.Pact.AspNetCore.Middlewares
{
    public static class PactServiceCollectionExtensions
    {
        public static IServiceCollection AddPactGeneration(this IServiceCollection services) =>
            services.AddPactGeneration(() => new PactOptions());

        public static IServiceCollection AddPactGeneration(
            this IServiceCollection services,
            Func<PactOptions> pactOptionsFactory)
        {
            services.AddTransient<PactSchemaService>(_ =>
                new PactSchemaService(pactOptionsFactory()));

            return services;
        }
    }
}
