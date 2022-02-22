namespace Byndyusoft.MaskedSerialization.UnitTests.Serilog
{
    using System.IO;
    using System.Linq;
    using AutoFixture;
    using global::Serilog;
    using global::Serilog.Sinks.TestCorrelator;
    using Infrastructure.Dtos;
    using MaskedSerialization.Serilog;
    using NUnit.Framework;

    [TestFixture]
    public class SampleTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _dto = _fixture.Create<TestDto>();

            var loggerConfiguration = Temp.GetLoggerConfiguration().WriteTo.Console().WriteTo.TestCorrelator();
            _logger = loggerConfiguration.CreateLogger();
        }

        private Fixture _fixture = default!;
        private TestDto _dto = default!;
        private ILogger _logger = default!;

        [Test]
        public void Test()
        {
            using (TestCorrelator.CreateContext())
            {
                // Arrange
                var template = "Deconstructed Dto {@TestDto}";

                // Act
                _logger.Information(template, _dto);

                // Assert
                var logEventsFromCurrentContext = TestCorrelator.GetLogEventsFromCurrentContext();

                var logEvents = logEventsFromCurrentContext.ToArray();
                Assert.That(logEvents.Length, Is.EqualTo(1));

                var logEvent = logEvents.Single();
                Assert.That(logEvent.MessageTemplate.Text, Is.EqualTo(template));

                using (var writer = new StringWriter())
                {
                    logEvent.RenderMessage(writer);
                    var loggedString = writer.ToString();

                    Assert.That(loggedString, Is.EqualTo(
                        "Deconstructed Dto TestDto {" +
                        $" Note: \"{_dto.Note}\"," +
                        " Password: \"*\"," +
                        $" Inner: TestInnerDto {{ Id: {_dto.Inner.Id}, Inn: \"*\" }}," +
                        " SecretInner: \"*\"" +
                        " }"));
                }
            }
        }
    }
}