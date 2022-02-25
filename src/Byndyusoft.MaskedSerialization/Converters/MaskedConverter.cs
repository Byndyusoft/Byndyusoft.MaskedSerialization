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
            if (typeMaskingInfo.IsMaskable == false)
                throw new InvalidOperationException("This converter is used only for maskable types");

            foreach (var propertyMaskingInfo in typeMaskingInfo.GetAllProperties())
            {
                var propertyInfo = propertyMaskingInfo.PropertyInfo;
                writer.WritePropertyName(propertyInfo.Name);
                if (propertyMaskingInfo.IsMasked)
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