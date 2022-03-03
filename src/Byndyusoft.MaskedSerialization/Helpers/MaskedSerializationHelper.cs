namespace Byndyusoft.MaskedSerialization.Helpers
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Converters;

    public static class MaskedSerializationHelper
    {
        public static JsonConverter MaskedConverterFactory { get; } = new MaskedConverterFactory();

        public static void SetupOptionsForMaskedSerialization(JsonSerializerOptions options)
        {
            options.Converters.Add(MaskedConverterFactory);
        }

        public static JsonSerializerOptions GetOptionsForMaskedSerialization()
        {
            var settings = new JsonSerializerOptions();
            SetupOptionsForMaskedSerialization(settings);

            return settings;
        }

        public static string SerializeWithMasking(object? value)
        {
            var settings = GetOptionsForMaskedSerialization();
            var serialized = JsonSerializer.Serialize(value, settings);

            return serialized;
        }
    }
}