namespace Byndyusoft.MaskedSerialization.UnitTests.Infrastructure.Dtos
{
    using Annotations.Attributes;

    public class TestNonMaskableDto
    {
        public string Note { get; set; } = default!;

        [Masked]
        public string Password { get; set; } = default!;
    }
}