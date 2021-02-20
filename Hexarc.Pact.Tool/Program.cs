using System;
using System.Text.Json;
using System.Threading.Tasks;
using Hexarc.Serialization.Union;
using Hexarc.Pact.Tool.Emitters;
using Hexarc.Pact.Tool.Internals;

namespace Hexarc.Pact.Tool
{
    public static class Program
    {
        public static async Task Main()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new UnionConverterFactory() }
            };

            var clientSettingsReader = new ClientSettingsReader(options);
            var clientSettingsCollection = await clientSettingsReader.Read();

            Console.WriteLine(ObjectDumper.Dump(clientSettingsCollection));
            if (clientSettingsCollection is null) return;

            foreach (var clientSettings in clientSettingsCollection)
            {
                var schemaReader = new SchemaReader(options);
                var schema = await schemaReader.ReadAsync(clientSettings.SchemaUri);
                if (schema is null) continue;

                var fileManager = new FileManager(clientSettings.OutputDirectory);
                var apiEmitter = new ApiEmitter(clientSettings, schema);
                var emittedApi = apiEmitter.Emit();
                fileManager.Save(emittedApi);
            }
        }
    }
}
