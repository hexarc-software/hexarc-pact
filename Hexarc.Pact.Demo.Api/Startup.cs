using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hexarc.Serialization.Union;
using Hexarc.Pact.AspNetCore.Middlewares;

namespace Hexarc.Pact.Demo.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddJsonOptions(configure =>
                {
                    configure.JsonSerializerOptions.Converters.Add(new UnionConverterFactory());
                    configure.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.AddPactGeneration();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePact();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
