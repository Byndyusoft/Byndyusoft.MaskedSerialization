namespace Byndyusoft.MaskedSerialization.UnitTests.Infrastructure.Dtos
{
    using Annotations.Attributes;

    public class TestCompanyDto
    {
        public long Id { get; set; }

        [Masked]
        public long Inn { get; set; }
    }
}