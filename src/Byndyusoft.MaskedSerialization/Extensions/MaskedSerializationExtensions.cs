namespace Byndyusoft.MaskedSerialization.Extensions
{
    using System.Text.Json;
    using Helpers;

    public static class MaskedSerializationExtensions
    {
        public static void SetupSettingsForMaskedSerialization(this JsonSerializerOptions options)
        {
            MaskedSerializationHelper.SetupOptionsForMaskedSerialization(options);
        }
    }
}