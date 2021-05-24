namespace Byndyusoft.MaskedSerialization.Newtonsoft.Extensions
{
    using global::Newtonsoft.Json;
    using Helpers;

    public static class MaskedSerializationExtensions
    {
        public static void SetupSettingsForMaskedSerialization(this JsonSerializerSettings settings)
        {
            MaskedSerializationHelper.SetupSettingsForMaskedSerialization(settings);
        }
    }
}