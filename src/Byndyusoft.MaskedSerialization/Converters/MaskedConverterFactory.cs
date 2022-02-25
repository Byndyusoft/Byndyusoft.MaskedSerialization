namespace Byndyusoft.MaskedSerialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Core.MaskingInfo;

    public class MaskedConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            var typeMaskingInfo = TypeMaskingInfoHelper.Get(typeToConvert);
            return typeMaskingInfo.IsMaskable;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var genericType = typeof(MaskedConverter<>).MakeGenericType(typeToConvert);
            var maskedConverter = (JsonConverter)Activator.CreateInstance(genericType);
            return maskedConverter;
        }
    }
}