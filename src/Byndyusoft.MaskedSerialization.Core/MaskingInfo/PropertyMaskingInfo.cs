namespace Byndyusoft.MaskedSerialization.Core.MaskingInfo
{
    using System.Reflection;

    public class PropertyMaskingInfo
    {
        public PropertyMaskingInfo(PropertyInfo propertyInfo, bool isMasked)
        {
            PropertyInfo = propertyInfo;
            IsMasked = isMasked;
        }

        public PropertyInfo PropertyInfo { get; }

        public bool IsMasked { get; }
    }
}