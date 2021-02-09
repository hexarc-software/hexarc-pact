using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hexarc.Annotations;

namespace Hexarc.Rpc.Demo.Api.Middlewares
{
    public sealed class UnionConverterFactory : JsonConverterFactory
    {
        public override Boolean CanConvert(Type typeToConvert) =>
            typeToConvert.GetCustomAttribute<UnionTagAttribute>(false) is not null;

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var converterType = typeof(UnionConverter<>).MakeGenericType(typeToConvert);
            return (JsonConverter?)Activator.CreateInstance(converterType);
        }
    }
}
