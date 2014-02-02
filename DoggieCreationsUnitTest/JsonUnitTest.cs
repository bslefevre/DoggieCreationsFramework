using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoggieCreationsUnitTest
{
    [TestClass]
    public class JsonUnitTest
    {
        [TestMethod]
        public void JsonTest_()
        {
            var json = "{  \"width\": \"920\",  \"height\": \"360\",  \"url\": \"http://www.doggiecreations.nl/wp-content/uploads/2011/12/DoggieCreations_Header.jpg\",  \"titleNoFormatting\": \"Doggie Creations | Starting to blog..!\",}";

            var result = JsonConvert.DeserializeObject<TestJsonClass>(json);

            Assert.AreEqual(920, result.width);
            Assert.AreEqual(360, result.height);
        }

        private class TestJsonClass
        {
            public int width { get; set; }
            public int height { get; set; }
            public string url { get; set; }
            public string titleNoFormatting { get; set; }
        }
    }
}
