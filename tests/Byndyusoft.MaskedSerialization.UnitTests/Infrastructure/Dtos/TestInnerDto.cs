namespace Byndyusoft.MaskedSerialization.UnitTests.Infrastructure.Dtos
{
    using Annotations.Attributes;

    [Maskable]
    public class TestInnerDto
    {
        public long Id { get; set; }

        [Masked]
        public long Inn { get; set; }
    }
}