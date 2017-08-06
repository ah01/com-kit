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

        [Test]
        public void MultiCharFlags()
        {
            ap.Parse(new[] { "-ab", "/cd", "e" });

            Assert.True(ap.CheckFlag("a"));
            Assert.True(ap.CheckFlag("b"));
            Assert.True(ap.CheckFlag("c"));
            Assert.True(ap.CheckFlag("d"));
            Assert.False(ap.CheckFlag("e"));
        }
    }
}
