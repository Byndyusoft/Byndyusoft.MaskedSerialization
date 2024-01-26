﻿namespace Byndyusoft.MaskedSerialization.Helpers
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;
    using Annotations.Attributes;
    using Core.MaskingInfo;
    using Converters;

    public static class MaskedSerializationHelper
    {
        public static void SetupOptionsForMaskedSerialization(JsonSerializerOptions options)
        {
            var resolver = options.TypeInfoResolver ??= new DefaultJsonTypeInfoResolver();
            if (resolver is DefaultJsonTypeInfoResolver defaultJsonTypeInfoResolver)
                defaultJsonTypeInfoResolver.Modifiers.Add(DetectMaskedMemberAttribute);
            else
                throw new InvalidOperationException($"Expected resolver of type {typeof(DefaultJsonTypeInfoResolver)}");
        }

        private static void DetectMaskedMemberAttribute(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Kind != JsonTypeInfoKind.Object)
                return;

            var typeMaskingInfo = TypeMaskingInfoHelper.Get(typeInfo.Type);
            if (typeMaskingInfo.HasMaskedProperties == false)
                return;

            foreach (var propertyInfo in typeInfo.Properties)
            {
                if (propertyInfo.AttributeProvider is { } provider &&
                    provider.IsDefined(typeof(MaskedAttribute), inherit: true))
                {
                    propertyInfo.CustomConverter = new MaskedConverterFactory();
                }
            }
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