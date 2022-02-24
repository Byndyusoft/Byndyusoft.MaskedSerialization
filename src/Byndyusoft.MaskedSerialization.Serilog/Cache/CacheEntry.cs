namespace Byndyusoft.MaskedSerialization.Serilog.Cache
{
    using Policies;

    public class CacheEntry
    {
        public CacheEntry(CacheEntryProperty[] properties)
        {
            Properties = properties;
        }

        public CacheEntryProperty[] Properties { get; }
    }
}