using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Hexarc.Pact.Protocol.Api;

namespace Hexarc.Pact.Tool.Utils
{
    public sealed class SchemaReader
    {
        private JsonSerializerOptions JsonSerializerOptions { get; }

        private HttpClient HttpClient { get; } = new();

        public SchemaReader(JsonSerializerOptions jsonSerializerOptions) =>
            this.JsonSerializerOptions = jsonSerializerOptions;

        public async Task<Schema?> ReadAsync(String schemaUri) =>
            await this.HttpClient.GetFromJsonAsync<Schema>(schemaUri, this.JsonSerializerOptions);
    }
}
