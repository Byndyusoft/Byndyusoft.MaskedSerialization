namespace Byndyusoft.MaskedSerialization.Converters
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

    public class JsonConverterPropertyWriterFactory<T>
    {
        private static readonly ConcurrentDictionary<PropertyInfo, JsonConverterPropertyWriter<T>> Writers = new();

        public static JsonConverterPropertyWriter<T> Create(PropertyInfo propertyInfo)
        {
            var jsonConverterPropertyWriter = Writers.GetOrAdd(propertyInfo, CreateWriter);
            return jsonConverterPropertyWriter;
        }

        private static JsonConverterPropertyWriter<T> CreateWriter(PropertyInfo propertyInfo)
        {
            var writerType = typeof(JsonConverterPropertyWriter<,>).MakeGenericType(typeof(T), propertyInfo.PropertyType);
            var writer = (JsonConverterPropertyWriter<T>)Activator.CreateInstance(writerType, propertyInfo);
            return writer;
        }
    }
}