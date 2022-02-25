namespace Byndyusoft.MaskedSerialization.Core.MaskingInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class TypeMaskingInfo
    {
        private readonly IReadOnlyDictionary<PropertyInfo, PropertyMaskingInfo> _propertyMaskingInfos;

        private TypeMaskingInfo(Type type, PropertyMaskingInfo[] properties, bool isMaskable)
        {
            Type = type;
            IsMaskable = isMaskable;
            _propertyMaskingInfos = properties.ToDictionary(i => i.PropertyInfo);
        }

        public Type Type { get; }

        public bool IsMaskable { get; }

        public static TypeMaskingInfo ForMaskable(Type type, PropertyMaskingInfo[] properties)
        {
            return new TypeMaskingInfo(type, properties, true);
        }

        public static TypeMaskingInfo ForNonMaskable(Type type)
        {
            return new TypeMaskingInfo(type, Array.Empty<PropertyMaskingInfo>(), false);
        }

        public IEnumerable<PropertyMaskingInfo> GetAllProperties()
        {
            if (IsMaskable == false)
                throw new InvalidOperationException("Type is not maskable. Properties are not accessible");

            return _propertyMaskingInfos.Values;
        }

        public bool IsMemberMasked(MemberInfo memberInfo)
        {
            if (IsMaskable && memberInfo is PropertyInfo propertyInfo)
            {
                if (_propertyMaskingInfos.TryGetValue(propertyInfo, out var propertyMaskingInfo))
                    return propertyMaskingInfo.IsMasked;
            }

            return false;
        }
    }
}