namespace Byndyusoft.MaskedSerialization.Serilog.Cache
{
    using System.Reflection;

    public class CacheEntryProperty
    {
        public CacheEntryProperty(PropertyInfo propertyInfo, bool isMasked)
        {
            PropertyInfo = propertyInfo;
            IsMasked = isMasked;
        }

        public PropertyInfo PropertyInfo { get; }

        public bool IsMasked { get; }
    }
}