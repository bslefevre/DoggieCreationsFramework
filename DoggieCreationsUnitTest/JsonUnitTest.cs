using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DoggieCreationsUnitTest
{
    [TestClass]
    public class JsonUnitTest
    {
        [TestMethod]
        public void JsonTest_Simple()
        {
            var json = "{  \"width\": \"920\",  \"height\": \"360\",  \"url\": \"http://www.doggiecreations.nl/wp-content/uploads/2011/12/DoggieCreations_Header.jpg\",  \"titleNoFormatting\": \"Doggie Creations | Starting to blog..!\",}";

            var result = JsonConvert.DeserializeObject<TestJsonClass>(json);

            Assert.AreEqual(920, result.Width);
            Assert.AreEqual(360, result.Height);
        }

        [TestMethod]
        public void JsonTest_WithCollection()
        {
            var json = "{ \"results\": [{\"test\": \"first Text\"}, {\"test\": \"second Text\"}], \"width\": \"920\",  \"height\": \"360\",  \"url\": \"www.google.nl\",  \"titleNoFormatting\": \"Title\"}";

            var result = JsonConvert.DeserializeObject<TestJsonClass>(json);
            Assert.AreEqual(2, result.InnerTestJsonClassCollection.Count);
        }

        private class TestJsonClass
        {
            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }
            
            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("titleNoFormatting")]
            public string Title { get; set; }

            [JsonProperty("results")]
            public Collection<InnerTestJsonClass> InnerTestJsonClassCollection { get; set; }
        }

        private class InnerTestJsonClass
        {
            [JsonProperty("test")]
            public string Property { get; set; }
        }
    }
}