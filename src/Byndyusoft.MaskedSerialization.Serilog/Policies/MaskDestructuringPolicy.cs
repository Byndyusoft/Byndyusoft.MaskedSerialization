namespace Byndyusoft.MaskedSerialization.Serilog.Policies
{
    using System.Linq;
    using System.Reflection;
    using Annotations;
    using Extensions;
    using global::Serilog.Core;
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
                return new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue("*"));

            var propertyValue = propertyInfo.GetValue(value);
            return new LogEventProperty(propertyInfo.Name, propertyValueFactory.CreatePropertyValue(propertyValue, true));
        }
    }
}