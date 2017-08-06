
using ComKit.Core.Cli;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComKit.Core.Tests.Cli
{
    [TestFixture]
    class ArgumentsParserTest
    {
        private ArgumentsParser ap;

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
