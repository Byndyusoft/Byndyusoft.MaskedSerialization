namespace Byndyusoft.MaskedSerialization.UnitTests.Newtonsoft.Helpers
{
    using AutoFixture;
    using Infrastructure.Dtos;
    using MaskedSerialization.Newtonsoft.Helpers;
    using NUnit.Framework;

    [TestFixture]
    public class MaskedSerializationHelperTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        private Fixture _fixture = default!;

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