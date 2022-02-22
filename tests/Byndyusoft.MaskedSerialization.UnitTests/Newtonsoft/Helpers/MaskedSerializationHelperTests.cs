namespace Byndyusoft.MaskedSerialization.UnitTests.Newtonsoft.Helpers
{
    using Annotations;
    using AutoFixture;
    using MaskedSerialization.Newtonsoft.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class MaskedSerializationHelperTests
    {
        private Fixture _fixture = default!;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void SerializeWithMasking_WithInner_SerializedWithMasks()
        {
            var dto = _fixture.Create<TestDto>();
            var expected =
                $"{{\"Note\":\"{dto.Note}\",\"Password\":\"*\",\"Inner\":{{\"Id\":{dto.Inner.Id},\"Inn\":\"*\"}},\"SecretInner\":\"*\"}}";

            var serialized = MaskedSerializationHelper.SerializeWithMasking(dto);

            Assert.That(serialized, Is.EqualTo(expected));
        }

        public class TestDto
        {
            public string Note { get; set; } = default!;

            [Masked]
            public string Password { get; set; } = default!;

            public TestInnerDto Inner { get; set; } = default!;

            [Masked]
            public TestInnerDto? SecretInner { get; set; } = default!;
        }

        public class TestInnerDto
        {
            public long Id { get; set; }

            [Masked]
            public long Inn { get; set; }
        }
    }
}