namespace Byndyusoft.MaskedSerialization.UnitTests.Infrastructure.Dtos
{
    using Annotations.Attributes;

    public class TestNonMaskableUserDto
    {
        public string Note { get; set; } = default!;

        [Masked]
        public TestCompanyDto Company { get; set; } = default!;
    }
}