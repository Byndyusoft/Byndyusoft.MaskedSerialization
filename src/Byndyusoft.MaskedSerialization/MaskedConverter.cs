namespace Byndyusoft.MaskedSerialization
{
    using System;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Annotations;
    using Annotations.Attributes;
    using Annotations.Consts;

    public class MaskedConverter<T> : JsonConverter<T>
        where T : class
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new InvalidOperationException("This converter is used only to write");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var propertyInfos = value.GetType().GetGetablePropertiesRecursively();
            foreach (var propertyInfo in propertyInfos)
            {
                writer.WritePropertyName(propertyInfo.Name);
                var maskedAttribute = propertyInfo.GetCustomAttribute<MaskedAttribute>();
                if (maskedAttribute != null)
                {
                    writer.WriteStringValue(MaskStrings.Default);
                }
                else
                {
                    var jsonConverter = options.GetConverter(propertyInfo.PropertyType);
                    var methodInfo = jsonConverter.GetType().GetMethod("Write");
                    if (methodInfo == null)
                        throw new InvalidOperationException($"Write method is not found in {jsonConverter.GetType().Name}");

                    var propertyValue = propertyInfo.GetValue(value);
                    methodInfo.Invoke(jsonConverter, new[] { writer, propertyValue, options });
                }
            }

            writer.WriteEndObject();
        }
    }
}