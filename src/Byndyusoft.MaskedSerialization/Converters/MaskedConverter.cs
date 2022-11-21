﻿namespace Byndyusoft.MaskedSerialization.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Annotations.Consts;

    public class MaskedConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new InvalidOperationException("This converter is used only to write");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(MaskStrings.Default);
        }
    }
}