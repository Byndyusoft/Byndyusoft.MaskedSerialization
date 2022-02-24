namespace Byndyusoft.MaskedSerialization
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class JsonFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsClass && typeToConvert.IsPrimitive == false && typeToConvert != typeof(string);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var genericType = typeof(MaskedConverter<>).MakeGenericType(typeToConvert);
            var maskedConverter = (JsonConverter)Activator.CreateInstance(genericType);
            return maskedConverter;
        }
    }
}