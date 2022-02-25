namespace Byndyusoft.MaskedSerialization.UnitTests.Serilog
{
    using System.IO;
    using System.Linq;
    using AutoFixture;
    using global::Serilog;
    using global::Serilog.Sinks.TestCorrelator;
    using Infrastructure.Dtos;
    using MaskedSerialization.Serilog.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class MaskDestructingPolicyTests
    {
        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            var loggerConfiguration = new LoggerConfiguration().WithMaskingPolicy().WriteTo.Console().WriteTo.TestCorrelator();
            _logger = loggerConfiguration.CreateLogger();
        }

        private Fixture _fixture = default!;
        private ILogger _logger = default!;

        [Test]
        public void Log_TestDto_LoggedDestructuredDataIsMasked()
        {
            using (TestCorrelator.CreateContext())
            {
                // Arrange
                var dto = _fixture.Create<TestUserDto>();
                var template = "Deconstructed Dto {@TestUserDto}";

                // Act
                _logger.Information(template, dto);

                // Assert
                AssertSingleLoggedString(template, "Deconstructed Dto TestUserDto {" +
                                                   $" Note: \"{dto.Note}\"," +
                                                   " Password: \"*\"," +
                                                   $" Company: TestCompanyDto {{ Id: {dto.Company.Id}, Inn: \"*\" }}," +
                                                   " SecretCompany: \"*\"" +
                                                   " }");
            }
        }

        [Test]
        public void Log_TestNonMaskableDto_LoggedDestructuredDataIsNotMasked()
        {
            using (TestCorrelator.CreateContext())
            {
                // Arrange
                var dto = _fixture.Create<TestNonMaskableDto>();
                var template = "Deconstructed Dto {@TestNonMaskableDto}";

                // Act
                _logger.Information(template, dto);

                // Assert
                AssertSingleLoggedString(template, "Deconstructed Dto TestNonMaskableDto {" +
                                                   $" Note: \"{dto.Note}\"," +
                                                   $" Password: \"{dto.Password}\"" +
                                                   " }");
            }
        }

        [Test]
        public void Log_TestNonMaskableUserDto_LoggedDestructuredCompanyIsMasked()
        {
            using (TestCorrelator.CreateContext())
            {
                // Arrange
                var dto = _fixture.Create<TestNonMaskableUserDto>();
                var template = "Deconstructed Dto {@TestNonMaskableUserDto}";

                // Act
                _logger.Information(template, dto);

                // Assert
                AssertSingleLoggedString(template, "Deconstructed Dto TestNonMaskableUserDto {" +
                                                   $" Note: \"{dto.Note}\"," +
                                                   $" Company: TestCompanyDto {{ Id: {dto.Company.Id}, Inn: \"*\" }}" +
                                                   " }");
            }
        }

        private void AssertSingleLoggedString(string template, string expectedLoggedString)
        {
            var logEventsFromCurrentContext = TestCorrelator.GetLogEventsFromCurrentContext();

            var logEvents = logEventsFromCurrentContext.ToArray();
            Assert.That(logEvents.Length, Is.EqualTo(1));

            var logEvent = logEvents.Single();
            Assert.That(logEvent.MessageTemplate.Text, Is.EqualTo(template));

            using var writer = new StringWriter();
            logEvent.RenderMessage(writer);
            var loggedString = writer.ToString();

            Assert.That(loggedString, Is.EqualTo(expectedLoggedString));
        }
    }
}