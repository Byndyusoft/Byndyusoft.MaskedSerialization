namespace Byndyusoft.MaskedSerialization.Serilog.Policies
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using Annotations.Attributes;
    using Annotations.Consts;
    using Cache;
    using Core.Extensions;
    using global::Serilog.Core;
    using global::Serilog.Debugging;
    using global::Serilog.Events;

    public class MaskDestructuringPolicy : IDestructuringPolicy
    {
        static readonly ConcurrentDictionary<Type, CacheEntry> Cache = new ConcurrentDictionary<Type, CacheEntry>();

        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue? result)
        {
            var type = value.GetType();

            var cacheEntry = Cache.GetOrAdd(type, GetCacheEntry);
            if (cacheEntry.IsMaskableType == false)
            {
                result = null;
                return false;
            }

            var logEventProperties =
                cacheEntry.Properties.Select(cacheEntryProperty => CreateLogEventProperty(value, cacheEntryProperty, propertyValueFactory));

            result = new StructureValue(logEventProperties, type.Name);
            return true;
        }

        private CacheEntry GetCacheEntry(Type type)
        {
            var propertyInfos = type.GetGetablePropertiesRecursively().ToArray();
            var cacheEntryProperties = propertyInfos.Select(GetCacheEntryProperty).ToArray();

            return new CacheEntry(cacheEntryProperties, IsTypeMaskable(type));
        }

        private CacheEntryProperty GetCacheEntryProperty(PropertyInfo propertyInfo)
        {
            var maskedAttribute = propertyInfo.GetCustomAttribute<MaskedAttribute>();
            var isMasked = maskedAttribute != null;
            return new CacheEntryProperty(propertyInfo, isMasked);
        }

        private bool IsTypeMaskable(Type type)
        {
            var cacheEntryClass = type.GetCustomAttribute<MaskableAttribute>();
            return cacheEntryClass != null;
        }

        private LogEventProperty CreateLogEventProperty(
            object value,
            CacheEntryProperty cacheEntryProperty,
            ILogEventPropertyValueFactory propertyValueFactory)
        {
            if (cacheEntryProperty.IsMasked)
                return new LogEventProperty(cacheEntryProperty.PropertyInfo.Name,
                    propertyValueFactory.CreatePropertyValue(MaskStrings.Default));

            var propertyValue = SafeGetPropertyValue(value, cacheEntryProperty.PropertyInfo);
            return new LogEventProperty(cacheEntryProperty.PropertyInfo.Name,
                propertyValueFactory.CreatePropertyValue(propertyValue, true));
        }

        private object SafeGetPropertyValue(object value, PropertyInfo propertyInfo)
        {
            try
            {
                return propertyInfo.GetValue(value);
            }
            catch (TargetInvocationException exception)
            {
                SelfLog.WriteLine("The property accessor {0} threw exception {1}", propertyInfo, exception);
                return $"Получение значения свойства вызвало исключение: {exception.InnerException!.GetType().Name}";
            }
        }
    }
}