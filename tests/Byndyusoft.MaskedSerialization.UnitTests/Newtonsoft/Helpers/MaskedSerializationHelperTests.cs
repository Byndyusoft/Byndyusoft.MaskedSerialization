namespace Byndyusoft.MaskedSerialization.UnitTests.Newtonsoft.Helpers
{
    using AutoFixture;
    using Infrastructure.Dtos;
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
    }
}