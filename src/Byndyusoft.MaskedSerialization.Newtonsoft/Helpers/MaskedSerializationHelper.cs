namespace Byndyusoft.MaskedSerialization.Newtonsoft.Helpers
{
    using global::Newtonsoft.Json;
    using global::Newtonsoft.Json.Serialization;
    using Serialization;

    public static class MaskedSerializationHelper
    {
        public static IContractResolver MaskedContractResolver { get; } = new MaskedContractResolver();

        public static void SetupSettingsForMaskedSerialization(JsonSerializerSettings settings)
        {
            settings.ContractResolver = MaskedContractResolver;
        }

        public static JsonSerializerSettings GetSettingsForMaskedSerialization()
        {
            var settings = new JsonSerializerSettings();
            SetupSettingsForMaskedSerialization(settings);

            return settings;
        }

        public static string SerializeWithMasking(object? value)
        {
            var settings = GetSettingsForMaskedSerialization();
            var serialized = JsonConvert.SerializeObject(value, settings);

            return serialized;
        }
    }
}