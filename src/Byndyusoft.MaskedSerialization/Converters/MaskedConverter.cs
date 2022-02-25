namespace Byndyusoft.MaskedSerialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Annotations.Consts;
    using Core.MaskingInfo;

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

            var typeMaskingInfo = TypeMaskingInfoHelper.Get(value.GetType());
            if (typeMaskingInfo.HasMaskedProperties == false)
                throw new InvalidOperationException("This converter is used only types with masked properties");

            foreach (var propertyMaskingInfo in typeMaskingInfo.GetAllProperties())
            {
                var propertyInfo = propertyMaskingInfo.PropertyInfo;
                var propertyInfoName = propertyInfo.Name;
                writer.WritePropertyName(options.PropertyNamingPolicy?.ConvertName(propertyInfoName) ?? propertyInfoName);
                if (propertyMaskingInfo.IsMasked)
                {
                    writer.WriteStringValue(MaskStrings.Default);
                }
                else
                {
                    var jsonConverter = options.GetConverter(propertyInfo.PropertyType);

                    var jsonConverterPropertyWriter = JsonConverterPropertyWriterFactory<T>.Create(propertyInfo);
                    jsonConverterPropertyWriter.Write(jsonConverter, writer, value, options);
                }
            }

            writer.WriteEndObject();
        }
    }
}