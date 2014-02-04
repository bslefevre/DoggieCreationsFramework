using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;

namespace DoggieCreationsFramework
{
    public class SearchClass
    {
        public static List<GoogleSearchResultClass> GoogleSearch(string search_expression)
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
                var jToken = o["responseData"];
                var resonseDataClass = jToken.ToObject<ResponseDataClass>();
                if (resonseDataClass != null)
                    valueCollection.AddRange(resonseDataClass.GoogleSearchResultCollection);
                if (valueCollection.Any()) break;
            }

            return valueCollection;
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

            imgName = RemoveInvalidFilePathCharacters(imgName);

            var filename = string.Format(@"{0}\images\{1}.jpg", BaseLocation, imgName);
            
            image.Save(filename);
            return image;
        }

        public static string RemoveInvalidFilePathCharacters(string filename, string replaceChar = null)
        {
            var regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
            return r.Replace(filename, string.IsNullOrEmpty(replaceChar) ? string.Empty : replaceChar);
        }

        private static string BaseLocation { get { return Environment.CurrentDirectory; } }

        public class ResponseDataClass
        {
            [JsonProperty("results")]
            public Collection<GoogleSearchResultClass> GoogleSearchResultCollection { get; set; }
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
