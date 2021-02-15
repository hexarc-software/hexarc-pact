using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Hexarc.Serialization.Union;
using Hexarc.Pact.Protocol.Types;
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
                var schema = await schemaReader.ReadAsync(clientSettings.SchemaUrl);
                if (schema is null) continue;
                Console.WriteLine(ObjectDumper.Dump(schema));

                DirectoryOperations.CreateOrClear(clientSettings.OutputDirectory);
                Directory.CreateDirectory(Path.Combine(clientSettings.OutputDirectory, "Models"));
                Directory.CreateDirectory(Path.Combine(clientSettings.OutputDirectory, "Controllers"));

                foreach (var type in schema.Types.OfType<DistinctType>())
                {
                    File.Create(
                        Path.Combine(Path.Combine(clientSettings.OutputDirectory, "Models", $"{type.Name}.cs")));
                }

                foreach (var controller in schema.Controllers)
                {
                    File.Create(
                        Path.Combine(Path.Combine(clientSettings.OutputDirectory, "Controllers", $"{controller.Name}.cs")));
                }
            }
        }
    }
}
