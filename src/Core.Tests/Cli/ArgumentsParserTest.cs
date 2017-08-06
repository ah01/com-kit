using ComKit.Core.Cli;
using NUnit.Framework;

namespace ComKit.Core.Tests.Cli
{
    [TestFixture]
    class ArgumentsParserTest
    {
        ArgumentsParser ap;

        [SetUp]
        protected void SetUp()
        {
            ap = new ArgumentsParser();
        }

        [Test]
        public void Flags()
        {
            ap.Parse(new[] { "-a", "/b", "c" });

            Assert.True(ap.CheckFlag("a"));
            Assert.True(ap.CheckFlag("b"));
            Assert.False(ap.CheckFlag("c"));
        }
    }
}
