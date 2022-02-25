namespace Byndyusoft.MaskedSerialization.UnitTests.SystemTextJson.Helpers
{
    using System;
    using AutoFixture;
    using Infrastructure.Dtos;
    using MaskedSerialization.Helpers;
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
        public void SerializeWithMasking_MaskableWithInner_SerializedWithMasks()
        {
            // Assert
            var dto = _fixture.Create<TestUserDto>();
            var expected =
                $"{{\"Note\":\"{dto.Note}\",\"Password\":\"*\",\"Company\":{{\"Id\":{dto.Company.Id},\"Inn\":\"*\"}},\"SecretCompany\":\"*\"}}";

            // Act
            var serialized = MaskedSerializationHelper.SerializeWithMasking(dto);
            Console.WriteLine(serialized);

            // Assert
            Assert.That(serialized, Is.EqualTo(expected));
        }
    }
}