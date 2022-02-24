namespace Byndyusoft.MaskedSerialization
{
    using System.Text.Json;

    public class Sample
    {
        public static JsonSerializerOptions GetJsonSerializerOptions()
        {
            var jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new JsonFactory());

            return jsonSerializerOptions;
        }
    }
}
