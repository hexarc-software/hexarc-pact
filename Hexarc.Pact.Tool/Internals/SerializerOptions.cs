namespace Hexarc.Pact.Tool.Internals;

using Hexarc.Serialization.Union;

public static class SerializerOptions
{
    public static readonly JsonSerializerOptions Default = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new UnionConverterFactory() }
    };
}
