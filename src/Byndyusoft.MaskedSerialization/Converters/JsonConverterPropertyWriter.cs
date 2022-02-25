namespace Byndyusoft.MaskedSerialization.Converters
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    
    public abstract class JsonConverterPropertyWriter<T>
    {
        public abstract void Write(JsonConverter jsonConverter, Utf8JsonWriter writer, T value, JsonSerializerOptions options);
    }

    public class JsonConverterPropertyWriter<T, TProperty> : JsonConverterPropertyWriter<T>
    {
        private readonly Func<T, TProperty> _propertyGetterFunc;

        public JsonConverterPropertyWriter(PropertyInfo propertyInfo)
        {
            var valueParameterExpression = Expression.Parameter(typeof(T));
            var propertyExpression = Expression.Property(valueParameterExpression, propertyInfo);
            _propertyGetterFunc = Expression.Lambda<Func<T, TProperty>>(propertyExpression, valueParameterExpression).Compile();
        }

        public override void Write(JsonConverter jsonConverter, Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var propertyValue = _propertyGetterFunc(value);
            if (jsonConverter is JsonConverter<TProperty> typedConverter)
                typedConverter.Write(writer, propertyValue, options);
            else
                throw new InvalidOperationException(
                    $"Json converter of type {jsonConverter.GetType().Name} is not convertible to {typeof(JsonConverter<TProperty>).Name}");
        }
    }
}