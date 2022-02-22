namespace Byndyusoft.MaskedSerialization.UnitTests.Infrastructure.Dtos
{
    using Annotations;

    public class TestDto
    {
        public string Note { get; set; } = default!;

        [Masked]
        public string Password { get; set; } = default!;

        public TestInnerDto Inner { get; set; } = default!;

        [Masked]
        public TestInnerDto? SecretInner { get; set; } = default!;
    }
}