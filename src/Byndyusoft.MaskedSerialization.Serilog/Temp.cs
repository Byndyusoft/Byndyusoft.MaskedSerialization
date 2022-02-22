namespace Byndyusoft.MaskedSerialization.Serilog
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Annotations;
    using global::Serilog;
    using global::Serilog.Core;
    using global::Serilog.Events;

    public class Temp
    {
        public static LoggerConfiguration GetLoggerConfiguration()
        {
            return new LoggerConfiguration().Destructure.With<MaskDestructuringPolicy>();
        }
    }

    public class MaskDestructuringPolicy : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            var type = value.GetType();
            var propertyInfos = type.GetPropertiesRecursively();
            var logEventProperties = propertyInfos.Select(propertyInfo => CreateLogEventProperty(value, propertyInfo, propertyValueFactory));

            result =  new StructureValue(logEventProperties, type.Name);
            return true;
        }

        private LogEventProperty CreateLogEventProperty(object value, PropertyInfo propertyInfo, ILogEventPropertyValueFactory propertyValueFactory)
        {
            var maskedAttribute = propertyInfo.GetCustomAttribute<MaskedAttribute>();
            if (maskedAttribute != null)
                return new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue("*"));

            var propertyValue = propertyInfo.GetValue(value);
            return new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(propertyValue, true));
        }
    }

    static class TypeExtensions
    {
        internal static IEnumerable<PropertyInfo> GetPropertiesRecursively(this Type type)
        {
            var seenNames = new HashSet<string>();

            var currentTypeInfo = type.GetTypeInfo();

            while (currentTypeInfo.AsType() != typeof(object))
            {
                var unseenProperties = currentTypeInfo.DeclaredProperties.Where(p => p.CanRead &&
                                                                                     p.GetMethod.IsPublic && !p.GetMethod.IsStatic &&
                                                                                     (p.Name != "Item" ||
                                                                                      p.GetIndexParameters().Length == 0) &&
                                                                                     !seenNames.Contains(p.Name));

                foreach (var propertyInfo in unseenProperties)
                {
                    seenNames.Add(propertyInfo.Name);
                    yield return propertyInfo;
                }

                var baseType = currentTypeInfo.BaseType;
                if (baseType == null)
                {
                    yield break;
                }

                currentTypeInfo = baseType.GetTypeInfo();
            }
        }
    }
}