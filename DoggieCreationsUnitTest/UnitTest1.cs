using DoggieCreationsFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace DoggieCreationsUnitTest
{
    [TestClass]
    public class DcHelpersUnitTest
    {
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
            var dic = ((DoggieCreationsUnitTestLogger)DcFrameworkBase.Logging).Logging;
            var message = dic.FirstOrDefault().Value;
            Assert.AreEqual("test - waarde", result);
        }

        [TestMethod]
        public void FormatString_FormatteerExpert_Equals()
        {
            var test = "waarde";
            var test2 = "waarde2";
            var result = "{test} - hallo".Formatteer(() => new[] {test, test2});
            Assert.AreEqual(2, ((DoggieCreationsUnitTestLogger)DcFrameworkBase.Logging).Logging.Count());
            Assert.AreEqual("waarde - hallo", result);
        }

        [TestMethod]
        public void TranslateVraag_NLtoES_AreEqual()
        {
            var vertaaldeTekst = TranslateClass.Translate(input: "Hoe gaat het?", from: "nl", to: "es");
            Assert.AreEqual("¿Cómo estás?", vertaaldeTekst);
        }
    }
}
