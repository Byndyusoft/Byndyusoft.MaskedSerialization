﻿namespace Byndyusoft.MaskedSerialization.Core.MaskingInfo
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Annotations.Attributes;

    public static class TypeMaskingInfoHelper
    {
        private static readonly ConcurrentDictionary<Type, TypeMaskingInfo> Cache = new ConcurrentDictionary<Type, TypeMaskingInfo>();

        public static TypeMaskingInfo Get(Type type)
        {
            var typeMaskingInfo = Cache.GetOrAdd(type, GetTypeMaskingInfo);
            return typeMaskingInfo;
        }

        private static TypeMaskingInfo GetTypeMaskingInfo(Type type)
        {
            var isTypeMaskable = IsTypeMaskable(type);
            if (isTypeMaskable == false)
                return TypeMaskingInfo.ForNonMaskable(type);

            var propertyInfos = GetGetablePropertiesRecursively(type).ToArray();
            var cacheEntryProperties = propertyInfos.Select(GetPropertyMaskingInfo).ToArray();

            return TypeMaskingInfo.ForMaskable(type, cacheEntryProperties);
        }

        private static PropertyMaskingInfo GetPropertyMaskingInfo(PropertyInfo propertyInfo)
        {
            var maskedAttribute = propertyInfo.GetCustomAttribute<MaskedAttribute>();
            var isMasked = maskedAttribute != null;
            return new PropertyMaskingInfo(propertyInfo, isMasked);
        }

        private static bool IsTypeMaskable(Type type)
        {
            var cacheEntryClass = type.GetCustomAttribute<MaskableAttribute>();
            return cacheEntryClass != null;
        }

        private static IEnumerable<PropertyInfo> GetGetablePropertiesRecursively(Type type)
        {
            var foundPropertyNames = new HashSet<string>();

            var currentTypeInfo = type.GetTypeInfo();

            while (currentTypeInfo.AsType() != typeof(object))
            {
                var unseenProperties = currentTypeInfo.DeclaredProperties.Where(
                    p => p.CanRead &&
                         p.GetMethod.IsPublic && !p.GetMethod.IsStatic &&
                         (p.Name != "Item" || p.GetIndexParameters().Length == 0) &&
                         !foundPropertyNames.Contains(p.Name));

                foreach (var propertyInfo in unseenProperties)
                {
                    foundPropertyNames.Add(propertyInfo.Name);
                    yield return propertyInfo;
                }

                var baseType = currentTypeInfo.BaseType;
                if (baseType == null)
                    yield break;

                currentTypeInfo = baseType.GetTypeInfo();
            }
        }
    }
}