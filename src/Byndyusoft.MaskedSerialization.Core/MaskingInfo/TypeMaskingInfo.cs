namespace Byndyusoft.MaskedSerialization.Core.MaskingInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class TypeMaskingInfo
    {
        private readonly IReadOnlyDictionary<PropertyInfo, PropertyMaskingInfo> _propertyMaskingInfos;

        public TypeMaskingInfo(Type type, PropertyMaskingInfo[] properties, bool isMaskable)
        {
            Type = type;
            IsMaskable = isMaskable;
            _propertyMaskingInfos = properties.ToDictionary(i => i.PropertyInfo);
        }

        public Type Type { get; }

        public IEnumerable<PropertyMaskingInfo> GetAllProperties() => _propertyMaskingInfos.Values;

        public bool IsMemberMasked(MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo propertyInfo)
            {
                if (_propertyMaskingInfos.TryGetValue(propertyInfo, out var propertyMaskingInfo))
                    return propertyMaskingInfo.IsMasked;
            }

            return false;
        }

        public bool IsMaskable { get; }
    }
}