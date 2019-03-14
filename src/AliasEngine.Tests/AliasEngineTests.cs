using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace AliasEngine.Tests
{
    public class AliasEngineTests
    {
        private AliasConverter _converter;

        [SetUp]
        public void SetUp()
        {
            _converter = new AliasConverter(new InMemoryAliasStore(), new NullLogger<AliasConverter>());
            _converter.AddAlias("/x multiple return words");
            _converter.AddAlias("/j /join {0}");
            _converter.AddAlias("/test /result {1} {2} {0}");
            _converter.AddAlias("/t /topic {0}-");
            _converter.AddAlias("/j2 /join {0} | /join {1}");
            _converter.AddAlias("/j3 /join {0} | /join {1} | /join {2}");
        }

        [Test]
        [TestCase("/x", ExpectedResult = "multiple return words")]
        public string CanRunAlias(string alias)
        {
            var result = _converter.Convert(alias);

            return result[0];
        }

        [Test]
        [TestCase("/j hello", ExpectedResult = "/join hello")]
        [TestCase("/t hello there", ExpectedResult = "/topic hello there")]
        [TestCase("/test first second third", ExpectedResult = "/result second third first")]
        public string CanRunAliasWithParameters(string alias)
        {
            var result = _converter.Convert(alias);

            return result[0];
        }

        [Test]
        [TestCase("/j2 first second", ExpectedResult = new string[] { "/join first", "/join second" })]
        [TestCase("/j3 first second third", ExpectedResult = new string[] { "/join first", "/join second", "/join third" })]
        public string[] CanRunAliasWithMultipleResults(string alias)
        {
            var result = _converter.Convert(alias);

            return result;
        }

        [Test]
        public void InvalidAliasThrows()
        {
            Assert.Throws<FaultyAliasException>(() => _converter.AddAlias("invalid"));
        }
    }
}
