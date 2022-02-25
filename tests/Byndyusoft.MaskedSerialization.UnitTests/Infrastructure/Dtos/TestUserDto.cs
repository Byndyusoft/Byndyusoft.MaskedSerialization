namespace Byndyusoft.MaskedSerialization.UnitTests.Infrastructure.Dtos
{
    using Annotations.Attributes;

    [Maskable]
    public class TestUserDto
    {
        public string Note { get; set; } = default!;

        [Masked]
        public string Password { get; set; } = default!;

        public TestCompanyDto Company { get; set; } = default!;

        [Masked]
        public TestCompanyDto? SecretCompany { get; set; } = default!;
    }
}