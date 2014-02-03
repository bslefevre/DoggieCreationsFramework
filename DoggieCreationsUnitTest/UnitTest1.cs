using DoggieCreationsFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Linq;

namespace DoggieCreationsUnitTest
{
    [TestClass]
    public class DcHelpersUnitTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            DcFrameworkBase.SetLoggingType = DcFrameworkBase.LoggerType.UnitTest;
        }
        
        [TestMethod]
        public void FormatString_GetLoggingMessage_Equals()
        {
            "asdf".Formatteer();
            var dic = ((DoggieCreationsUnitTestLogger)DcFrameworkBase.Logging).Logging;
            var message = dic["asdf"].Message;
            Assert.AreEqual("asdf: System.String", message);
        }

        [TestMethod]
        public void FormatString_FormatteerNormal_Equals()
        {
            var result = "{0} - waarde".Formatteer("test");
            Assert.AreEqual(1, ((DoggieCreationsUnitTestLogger)DcFrameworkBase.Logging).Logging.Count());
            Assert.AreEqual("test - waarde", result);
        }

        [TestMethod]
        public void FormatString_WithTranslate_Equals()
        {
            var first = "hallo";
            var second = TranslateClass.Translate(first, "nl", "en");
            var result = "'{first}' is in het Engels '{second}'".Formatteer(() => new[] { first, second });
            Assert.AreEqual(2, ((DoggieCreationsUnitTestLogger)DcFrameworkBase.Logging).Logging.Count());
            Assert.AreEqual("'hallo' is in het Engels 'hello'", result);
        }

        [TestMethod]
        public void FormatString_FormatteerExpert_Equals()
        {
            var test = "waarde";
            var test2 = "hallo";
            var result = "{test} - {test2}".Formatteer(() => new [] {test, test2});
            Assert.AreEqual(2, ((DoggieCreationsUnitTestLogger)DcFrameworkBase.Logging).Logging.Count());
            Assert.AreEqual("waarde - hallo", result);
        }

        [TestMethod]
        public void TranslateVraag_NLtoES_AreEqual()
        {
            var vertaaldeTekst = TranslateClass.Translate(input: "Hoe gaat het?", from: "nl", to: "es");
            Assert.AreEqual("¿Cómo estás?", vertaaldeTekst);
        }

        [TestMethod]
        public void ParseWaarde_StringToInt_AreEqual()
        {
            var number = DcType<int>.ParseWaarde("1");
            Assert.IsTrue(((DoggieCreationsUnitTestLogger)DcFrameworkBase.Logging).Logging.ContainsKey(number));
            Assert.AreEqual(1, number);
        }

        [TestMethod]
        public void SearchTest_()
        {
            var result = SearchClass.GoogleSearch2("doggie creations.nl logo");
            
            //foreach (DoggieCreationsFramework.SearchClass.SearchResult searchResult in result)
            //{
                
            //    SearchClass.GetImageFromUrl(searchResult.title, searchResult.url);
                
            //}        
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ((DoggieCreationsUnitTestLogger)DcFrameworkBase.Logging).Logging.Clear();
        }
    }
}
