using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using DoggieCreationsFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoggieCreationsUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var result = "asf{0}".Formatteer();
            var applicationExceptions = DcString.Logging;
            var message = applicationExceptions[result].Message;
            Assert.AreEqual("Damn", message);
        }
    }
}
