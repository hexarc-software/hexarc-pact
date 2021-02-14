using System;
using System.Net;
using System.Text.Json;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Serialization.Union;

namespace Hexarc.Pact.Tool
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Welcome to pact");
            var web = new WebClient();
            var raw = web.DownloadString(new Uri("https://hexarc-demo-api.herokuapp.com/pact/schema"));
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new UnionConverterFactory() }
            };
            var schema = JsonSerializer.Deserialize<Schema>(raw, options);
            Console.WriteLine(ObjectDumper.Dump(schema));
        }
    }
}
