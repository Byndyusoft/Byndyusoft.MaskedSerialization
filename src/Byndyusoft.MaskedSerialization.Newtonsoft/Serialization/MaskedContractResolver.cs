namespace Byndyusoft.MaskedSerialization.Newtonsoft.Serialization
{
    using System.Reflection;
    using Annotations.Attributes;
    using global::Newtonsoft.Json;
    using global::Newtonsoft.Json.Serialization;

    public class MaskedContractResolver : DefaultContractResolver
    {
        private MaskConverter MaskConverter => new MaskConverter();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            var maskableAttribute = member.DeclaringType?.GetCustomAttribute<MaskableAttribute>();
            if (maskableAttribute == null)
                return property;

            var maskLoggingAttribute = member.GetCustomAttribute<MaskedAttribute>();
            if (maskLoggingAttribute != null)
                property.Converter = MaskConverter;

            return property;
        }
    }
}