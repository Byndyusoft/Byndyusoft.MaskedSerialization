namespace Byndyusoft.MaskedSerialization.Serilog.Cache
{
    public class CacheEntry
    {
        public CacheEntry(CacheEntryProperty[] properties, bool isMaskableType)
        {
            Properties = properties;
            IsMaskableType = isMaskableType;
        }

        public CacheEntryProperty[] Properties { get; }

        public bool IsMaskableType { get; }
    }
}