namespace Byndyusoft.MaskedSerialization.Newtonsoft.Serialization
{
    using System;
    using Annotations.Consts;
    using global::Newtonsoft.Json;

    public class MaskConverter : JsonConverter
    {
        public override bool CanWrite => true;

        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue(MaskStrings.Default);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}