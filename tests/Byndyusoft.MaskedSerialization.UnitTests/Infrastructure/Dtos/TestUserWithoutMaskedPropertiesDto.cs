namespace Byndyusoft.MaskedSerialization.UnitTests.Infrastructure.Dtos
{
    public class TestUserWithoutMaskedPropertiesDto
    {
        public string Note { get; set; } = default!;

        public TestCompanyDto Company { get; set; } = default!;
    }
}