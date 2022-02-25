﻿namespace Byndyusoft.MaskedSerialization.Serilog.Policies
{
    using System.Linq;
    using System.Reflection;
    using Annotations.Consts;
    using Core.MaskingInfo;
    using global::Serilog.Core;
    using global::Serilog.Debugging;
    using global::Serilog.Events;

    public class MaskDestructuringPolicy : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue? result)
        {
            var type = value.GetType();

            var typeMaskingInfo = TypeMaskingInfoHelper.Get(type);
            if (typeMaskingInfo.IsMaskable == false)
            {
                result = null;
                return false;
            }

            var logEventProperties = typeMaskingInfo
                .GetAllProperties()
                .Select(propertyMaskingInfo => CreateLogEventProperty(value, propertyMaskingInfo, propertyValueFactory));

            result = new StructureValue(logEventProperties, type.Name);
            return true;
        }

        private LogEventProperty CreateLogEventProperty(
            object value,
            PropertyMaskingInfo propertyMaskingInfo,
            ILogEventPropertyValueFactory propertyValueFactory)
        {
            if (propertyMaskingInfo.IsMasked)
                return new LogEventProperty(propertyMaskingInfo.PropertyInfo.Name,
                    propertyValueFactory.CreatePropertyValue(MaskStrings.Default));

            var propertyValue = SafeGetPropertyValue(value, propertyMaskingInfo.PropertyInfo);
            return new LogEventProperty(propertyMaskingInfo.PropertyInfo.Name,
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