using Hexarc.Serialization.Union;
using Hexarc.Pact.AspNetCore.Middlewares;
using Hexarc.Pact.Demo.Api.Middlewares;

namespace Hexarc.Pact.Demo.Api;

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
            });
        services.AddPactGeneration();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseForwardedHeaders();
        app.EnforceSslForForwardedProto();
        app.UsePact();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
