namespace Byndyusoft.MaskedSerialization.Converters
{
    using System;
    using System.Collections.Concurrent;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Core.MaskingInfo;

    public class MaskedConverterFactory : JsonConverterFactory
    {
        private static readonly ConcurrentDictionary<Type, JsonConverter>
            MaskedConverters = new ConcurrentDictionary<Type, JsonConverter>();

        public override bool CanConvert(Type typeToConvert)
        {
            var typeMaskingInfo = TypeMaskingInfoHelper.Get(typeToConvert);
            return typeMaskingInfo.IsMaskable;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var maskedConverter = MaskedConverters.GetOrAdd(typeToConvert, CreateMaskedConverter);
            return maskedConverter;
        }

        private JsonConverter CreateMaskedConverter(Type typeToConvert)
        {
            var genericType = typeof(MaskedConverter<>).MakeGenericType(typeToConvert);
            var maskedConverter = (JsonConverter)Activator.CreateInstance(genericType);
            return maskedConverter;
        }
    }
}