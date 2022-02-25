namespace Byndyusoft.MaskedSerialization.UnitTests.Newtonsoft.Helpers
{
    using System;
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
        public void SerializeWithMasking_HasMaskedPropertiesWithInner_SerializedWithMasks()
        {
            // Arrange
            var dto = _fixture.Create<TestUserDto>();
            var expected =
                $"{{\"Note\":\"{dto.Note}\",\"Password\":\"*\",\"Company\":{{\"Id\":{dto.Company.Id},\"Inn\":\"*\"}},\"SecretCompany\":\"*\"}}";

            // Act
            var serialized = MaskedSerializationHelper.SerializeWithMasking(dto);
            Console.WriteLine(serialized);

            // Assert
            Assert.That(serialized, Is.EqualTo(expected));
        }

        [Test]
        public void SerializeWithMasking_DoesNotHaveMaskedProperties_SerializedWithoutMasks()
        {
            // Arrange
            var dto = _fixture.Create<TestWithoutMaskedPropertiesDto>();
            var expected = $"{{\"Note\":\"{dto.Note}\",\"Password\":\"{dto.Password}\"}}";

            // Act
            var serialized = MaskedSerializationHelper.SerializeWithMasking(dto);
            Console.WriteLine(serialized);

            // Assert
            Assert.That(serialized, Is.EqualTo(expected));
        }

        [Test]
        public void SerializeWithMasking_InnerHasMaskedProperties_SerializedInnerWithMasks()
        {
            // Arrange
            var dto = _fixture.Create<TestUserWithoutMaskedPropertiesDto>();
            var expected = $"{{\"Note\":\"{dto.Note}\",\"Company\":{{\"Id\":{dto.Company.Id},\"Inn\":\"*\"}}}}";

            // Act
            var serialized = MaskedSerializationHelper.SerializeWithMasking(dto);
            Console.WriteLine(serialized);

            // Assert
            Assert.That(serialized, Is.EqualTo(expected));
        }
    }
}