using DoggieCreationsFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void TranslateVraag_NLtoES_AreEqual()
        {
            var vertaaldeTekst = TranslateClass.Translate(input: "Hoe gaat het?", from: "nl", to: "es");
            Assert.AreEqual("¿Cómo estás?", vertaaldeTekst);
        }
    }
}
