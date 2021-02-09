using System;
using System.Buffers;
using System.Text.Json;

namespace Hexarc.Rpc.Demo.Api.Middlewares
{
    internal static class JsonExtensions
    {
        public static Object? ToObject(this JsonElement element, Type type, JsonSerializerOptions options)
        {
            var bufferWriter = new ArrayBufferWriter<Byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter))
            {
                element.WriteTo(writer);
            }
            return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, type, options);
        }

        public static Object? ToObject(this JsonDocument document, Type type, JsonSerializerOptions options)
        {
            if (document is null) throw new ArgumentNullException(nameof(document));
            return document.RootElement.ToObject(type, options);
        }
    }
}
