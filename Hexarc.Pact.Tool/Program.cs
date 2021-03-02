using System;
using System.Threading.Tasks;
using Hexarc.Pact.Tool.Emitters;
using Hexarc.Pact.Tool.Internals;

namespace Hexarc.Pact.Tool
{
    public static class Program
    {
        public static async Task Main()
        {
            var clientSettingsReader = new ClientSettingsReader(SerializerOptions.Default);
            var clientSettingsCollection = await clientSettingsReader.Read();

            if (clientSettingsCollection is null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No pact.json config found. Nothing to generate.");
                return;
            }

            foreach (var clientSettings in clientSettingsCollection)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Generating API client for given settings.");
                Console.ResetColor();
                Console.WriteLine(ObjectDumper.Dump(clientSettings));

                var schemaReader = new SchemaReader(SerializerOptions.Default);
                var schema = await schemaReader.ReadAsync(clientSettings.SchemaUri, clientSettings.Scopes);
                if (schema is null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Could not read schema.");
                    continue;
                }

                var fileManager = new FileManager(clientSettings.OutputDirectory);
                var apiEmitter = new ApiEmitter(clientSettings, schema);
                var emittedApi = apiEmitter.Emit();
                fileManager.Save(emittedApi);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("API client successfully generated.");
            }
        }
    }
}
