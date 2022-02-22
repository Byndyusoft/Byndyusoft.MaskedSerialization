namespace Byndyusoft.MaskedSerialization.Serilog.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class TypeExtensions
    {
        internal static IEnumerable<PropertyInfo> GetGetablePropertiesRecursively(this Type type)
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