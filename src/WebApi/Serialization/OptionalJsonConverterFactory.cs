using Application.Common.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApi.Serialization
{
    public class OptionalJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var itemType = typeToConvert.GetGenericArguments()[0];
            var converterType = typeof(OptionalJsonConverter<>).MakeGenericType(itemType);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }

        private sealed class OptionalJsonConverter<T> : JsonConverter<Optional<T>>
        {
            public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                try
                {

                    var value = JsonSerializer.Deserialize<T>(ref reader, options);
                    return new Optional<T>(value);
                }
                catch (JsonException)
                {
                    // Gracefully handle bad input
                    return new Optional<T>(); // treat as unassigned
                }
            }

            public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
            {
                if (!value.IsAssigned)
                {
                    writer.WriteNullValue();
                    return;
                }

                // Serialize the inner value, not the wrapper
                JsonSerializer.Serialize(writer, value.Value, options);
            }
        }

    }
}
