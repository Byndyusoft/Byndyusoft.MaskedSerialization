namespace Byndyusoft.MaskedSerialization.Converters
{
    using System;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization.Metadata;
    using Core.MaskingInfo;

    public class MaskedJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
    {
        private readonly PropertyInfo _memberInfoProperty;
        private readonly MaskedConverterFactory _maskedConverterFactory;

        public MaskedJsonTypeInfoResolver()
        {
            var memberInfoProperty = typeof(JsonPropertyInfo).GetProperty("MemberInfo",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null, 
                typeof(MemberInfo), 
                Array.Empty<Type>(), 
                null);
            if (memberInfoProperty == null)
                throw new Exception($"Property with name MemberInfo is not found in type {nameof(JsonPropertyInfo)}");

            _memberInfoProperty = memberInfoProperty;
            _maskedConverterFactory = new MaskedConverterFactory();
        }

        public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
        {
            var jsonTypeInfo = base.GetTypeInfo(type, options);

            foreach (var jsonPropertyInfo in jsonTypeInfo.Properties)
                ProcessProperty(type, jsonPropertyInfo);

            return jsonTypeInfo;
        }

        private void ProcessProperty(Type type, JsonPropertyInfo jsonPropertyInfo)
        {
            var typeMaskingInfo = TypeMaskingInfoHelper.Get(type);
            if (typeMaskingInfo.HasMaskedProperties == false)
                return;

            var memberInfo = _memberInfoProperty.GetValue(jsonPropertyInfo) as MemberInfo;
            if (memberInfo == null)
                return;

            if (typeMaskingInfo.IsMemberMasked(memberInfo))
                jsonPropertyInfo.CustomConverter =
                    _maskedConverterFactory.CreateConverter(jsonPropertyInfo.PropertyType, jsonPropertyInfo.Options);
        }
    }
}