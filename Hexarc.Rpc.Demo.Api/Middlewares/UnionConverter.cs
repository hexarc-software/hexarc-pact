using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hexarc.Annotations;

namespace Hexarc.Rpc.Demo.Api.Middlewares
{
    public sealed class UnionConverter<T> : JsonConverter<T> where T : class
    {
        private String TagPropertyName { get; }

        private Dictionary<String, Type> UnionTypes { get; }

        public UnionConverter()
        {
            var type = typeof(T);
            var unionTag = type.GetCustomAttribute<UnionTagAttribute>();
            if (unionTag is null) throw new InvalidOperationException();

            var concreteTypeFactory = type.CreateConcreteTypeFactory();
            this.TagPropertyName = unionTag.TagPropertyName;
            this.UnionTypes = type.GetCustomAttributes<UnionCaseAttribute>()
                .ToDictionary(k => k.TagPropertyValue, e => concreteTypeFactory(e.CaseType));
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var propertyName = options.PropertyNamingPolicy?.ConvertName(this.TagPropertyName) ?? this.TagPropertyName;
            var property = document.RootElement.GetProperty(propertyName);
            var type = this.UnionTypes[property.GetString() ?? throw new InvalidOperationException()];
            return (T?)document.ToObject(type, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) =>
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
