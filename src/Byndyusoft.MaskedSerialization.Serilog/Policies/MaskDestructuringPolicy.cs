namespace Byndyusoft.MaskedSerialization.Serilog.Policies
{
    using System.Linq;
    using System.Reflection;
    using Annotations;
    using Core.Extensions;
    using global::Serilog.Core;
    using global::Serilog.Debugging;
    using global::Serilog.Events;

    public class MaskDestructuringPolicy : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            var type = value.GetType();
            var propertyInfos = type.GetGetablePropertiesRecursively();
            var logEventProperties =
                propertyInfos.Select(propertyInfo => CreateLogEventProperty(value, propertyInfo, propertyValueFactory));

            result = new StructureValue(logEventProperties, type.Name);
            return true;
        }

        private LogEventProperty CreateLogEventProperty(object value, PropertyInfo propertyInfo,
            ILogEventPropertyValueFactory propertyValueFactory)
        {
            var maskedAttribute = propertyInfo.GetCustomAttribute<MaskedAttribute>();
            if (maskedAttribute != null)
                return new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(MaskStrings.Default));

            var propertyValue = SafeGetPropertyValue(value, propertyInfo);
            return new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(propertyValue, true));
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