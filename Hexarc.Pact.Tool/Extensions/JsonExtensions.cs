namespace Hexarc.Pact.Tool.Extensions;

public static class JsonExtensions
{
    public static T? ToObject<T>(this JsonElement element, JsonSerializerOptions options)
    {
        var bufferWriter = new ArrayBufferWriter<Byte>();
        using (var writer = new Utf8JsonWriter(bufferWriter)) element.WriteTo(writer);
        return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
    }

    public static T? ToObject<T>(this JsonDocument document, JsonSerializerOptions options)
    {
        if (document is null) throw new ArgumentNullException(nameof(document));
        return document.RootElement.ToObject<T>(options);
    }
}
