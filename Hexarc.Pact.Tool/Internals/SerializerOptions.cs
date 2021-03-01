using System.Text.Json;
using Hexarc.Serialization.Union;

namespace Hexarc.Pact.Tool.Internals
{
    public static class SerializerOptions
    {
        public static readonly JsonSerializerOptions Default = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new UnionConverterFactory() }
        };
    }
}
