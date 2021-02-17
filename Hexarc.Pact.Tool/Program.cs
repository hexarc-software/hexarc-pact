using System;
using System.IO;
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
            Console.WriteLine("Welcome to pact");
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new UnionConverterFactory() }
            };

            var clientSettingsReader = new ClientSettingsReader(options);
            var clientSettingsCollection = await clientSettingsReader.Read();
            Console.WriteLine(ObjectDumper.Dump(clientSettingsCollection));
            if (clientSettingsCollection is null) return;

            var schemaReader = new SchemaReader(options);


            foreach (var clientSettings in clientSettingsCollection)
            {
                var schema = await schemaReader.ReadAsync(clientSettings.SchemaUri);
                if (schema is null) continue;

                DirectoryOperations.CreateOrClear(clientSettings.OutputDirectory);
                Directory.CreateDirectory(Path.Combine(clientSettings.OutputDirectory, "Models"));
                Directory.CreateDirectory(Path.Combine(clientSettings.OutputDirectory, "Controllers"));

                var apiEmitter = new ApiEmitter(clientSettings, schema);

                foreach (var type in apiEmitter.EmitTypes())
                {
                    var path = Path.Combine(Path.Combine(clientSettings.OutputDirectory, "Models", type.FileName));
                    await using var file = File.CreateText(path);
                    type.SourceText.Write(file);
                }

                foreach (var controller in apiEmitter.EmitControllers())
                {
                    var path = Path.Combine(Path.Combine(clientSettings.OutputDirectory, "Controllers", controller.FileName));
                    await using var file = File.CreateText(path);
                    controller.SourceText.Write(file);
                }
            }
        }
    }
}
