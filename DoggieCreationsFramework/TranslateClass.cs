using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Net;
using System.Threading;

namespace DoggieCreationsFramework
{
    public static class TranslateClass
    {
        public static string Translate(string input, string from, string to)
        {
            var languagePair = string.Format("{0}|{1}", from, to);

            var url = String.Format("http://www.google.com/translate_t?hl=en&ie=UTF8&text={0}&langpair={1}", input, languagePair);
            string result;

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.Default;
                result = webClient.DownloadString(url);
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Thread.Sleep(100);
            var selectSingleNode = doc.DocumentNode.SelectSingleNode(string.Format("//span[@title='{0}']", input));
            return selectSingleNode != null ? selectSingleNode.InnerText : string.Empty;
        }
    }
}
