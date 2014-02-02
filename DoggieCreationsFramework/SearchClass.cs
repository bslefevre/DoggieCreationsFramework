using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DoggieCreationsFramework
{
    public class SearchClass
    {
        public static List<SearchResult> GoogleSearch(string search_expression)
        {
            var url_template = @"http://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=large&safe=active&q={0}&start={1}";
            Uri search_url;
            int[] offsets = { 0, 8, 16, 24, 32, 40, 48 };

            var valueCollection = new List<string>();

            foreach (var offset in offsets)
            {
                search_url = new Uri(string.Format(url_template, search_expression, offset));
                var page = new WebClient().DownloadString(search_url);
                var o = (JObject)JsonConvert.DeserializeObject(page);
                foreach (KeyValuePair<string, JToken> keyValuePair in o)
                {
                    if (keyValuePair.Key == "responseData")
                    {
                        if (!keyValuePair.Value.HasValues) continue;

                        var results = keyValuePair.Value["results"];
                        var values = results.Values<string>("url");
                        var titles = results.Values<string>("title");
                        valueCollection.AddRange(values);
                    }
                    if (valueCollection.Any()) break;
                }

                if (valueCollection.Any()) break;
            }

            return valueCollection.Select(s => new SearchResult(s, null, null, SearchResult.FindingEngine.google)).ToList();
        }

        public static Image GetImageFromUrl(string imgName, string imgUrl)
        {
            if (string.IsNullOrEmpty(imgUrl)) return null;
            var webClient = new WebClient();
            var imageBytes = new byte[] { };
            try
            {
                imageBytes = webClient.DownloadData(imgUrl);
            }
            catch (WebException exception)
            {

            }

            return imageBytes.Count() > 0 ? ByteArrayToImage(imageBytes, imgName) : null;
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn, string imgName)
        {
            if (byteArrayIn == null) return null;
            Image image = null;
            try
            {
                image = Image.FromStream(new MemoryStream(byteArrayIn));
            }
            catch (ArgumentException)
            {

            }

            if (image == null) return null;

            if (!Directory.Exists(string.Format(@"{0}\images", BaseLocation)))
                Directory.CreateDirectory(string.Format(@"{0}\images", BaseLocation));
            image.Save(string.Format(@"{0}\images\{1}.jpg", BaseLocation, string.IsNullOrEmpty(imgName) ? "test" : imgName));
            return image;
        }

        private static string BaseLocation { get { return Environment.CurrentDirectory; } }

        public class SearchResult
        {
            public string url;
            public string title;
            public string content;
            public FindingEngine engine;

            public enum FindingEngine { google, bing, google_and_bing };

            public SearchResult(string url, string title, string content, FindingEngine engine)
            {
                this.url = url;
                this.title = title;
                this.content = content;
                this.engine = engine;
            }
        }

        public static List<GoogleSearchResultClass> GoogleSearch2(string search_expression)
        {
            var url_template = @"http://ajax.googleapis.com/ajax/services/search/images?v=1.0&rsz=large&safe=active&q={0}&start={1}";
            Uri search_url;
            int[] offsets = { 0, 8, 16, 24, 32, 40, 48 };

            var valueCollection = new List<GoogleSearchResultClass>();

            foreach (var offset in offsets)
            {
                search_url = new Uri(string.Format(url_template, search_expression, offset));
                var page = new WebClient().DownloadString(search_url);
                var o = (JObject)JsonConvert.DeserializeObject(page);
                foreach (KeyValuePair<string, JToken> keyValuePair in o)
                {
                    if (keyValuePair.Key == "responseData")
                    {
                        if (!keyValuePair.Value.HasValues) continue;
                        JTokenReader r = new JTokenReader(keyValuePair.Value);
                        var value = r.Value;
                        while (r.Read()){
                            var val = r.Value;
                        }

                        //JsonConvert.DeserializeObject<GoogleSearchResultClass>(keyValuePair.Value)

                        //var results = keyValuePair.Value["results"];
                        //var values = results.Values<string>("url");
                        //var titles = results.Values<string>("title");
                        //valueCollection.AddRange(values);
                    }
                    if (valueCollection.Any()) break;
                }

                if (valueCollection.Any()) break;
            }

            return valueCollection;
        }

        public class GoogleSearchResultClass
        {
            public string GsearchResultClass;
            public int width;
            public int height;
            public string imageId;
            public int tbWidth;
            public int tbHeight;
            public string unescapedUrl;
            public string url;
            public string visibleUrl;
            public string title;
            public string titleNoFormatting;
            public string originalContextUrl;
            public string content;
            public string contentNoFormatting;
            public string tbUrl;
        }
    }
}
