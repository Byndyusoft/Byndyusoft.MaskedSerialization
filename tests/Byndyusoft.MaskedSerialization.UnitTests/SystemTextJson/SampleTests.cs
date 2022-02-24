namespace Byndyusoft.MaskedSerialization.UnitTests.SystemTextJson
{
    using System;
    using System.Text.Json;
    using AutoFixture;
    using Infrastructure.Dtos;
    using NUnit.Framework;

    [TestFixture]
    public class SampleTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _jsonSerializerOptions = Sample.GetJsonSerializerOptions();
        }

        private Fixture _fixture = default!;
        private JsonSerializerOptions _jsonSerializerOptions = default!;

        [Test]
        public void Test()
        {
            var testDto = _fixture.Create<TestDto>();
            var serialized = JsonSerializer.Serialize(testDto, _jsonSerializerOptions);

            Console.WriteLine(serialized);
        }
    }
}