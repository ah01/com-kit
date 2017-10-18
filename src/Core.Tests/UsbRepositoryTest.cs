using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComKit.Core.Tests
{
    [TestFixture]
    public class UsbRepositoryTest
    {
        /// <summary>
        /// Basic test, just try to load file
        /// </summary>
        [Test]
        public void InitializeReporitoryTest()
        {
            // arduino uno
            var vid = 0x2341;
            var pid = 0x0001;

            var resutl = UsbRepository.FindDevice(vid, pid);

            Assert.IsNotNull(resutl);
        }
    }
}
