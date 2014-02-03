using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;
using System.Windows.Forms;

namespace DoggieCreationsFramework
{
    public static class DcString
    {
        public static string Formatteer(this string @string, params object[] keyWords)
        {
            var formattedString = string.Format(@string, keyWords);

            DcType<string>.Formatteer(formattedString);
            return formattedString;
        }

        public static string Formatteer(this string @string, Expression<Func<object[]>> expression)
        {
            var propertyNameAndValueDictionary = MemberInfoGetting.GetPropertyNameAndValue(expression);

            foreach (var kvp in propertyNameAndValueDictionary)
            {
                @string = @string.Replace(string.Format("{{{0}}}", kvp.Key), kvp.Value.ToString());
            }

            return @string;
        }

        /// <summary>
        /// To split the string in pieces by NewRule and end of a sentence.
        /// </summary>
        /// <param name="string">List of sentences to translate.</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitString(this string @string)
        {
            var splittedStringList = new List<string>();

            var splittedTextArray = @string.Split('\n');

            foreach (var s in splittedTextArray)
            {
                var nr = 0;
                while (s.IndexOf(".", nr) > -1 || s.IndexOf(".\r", nr) > -1)
                {
                    var ir = s.IndexOf(".\r", nr);
                    var ip = s.IndexOf(".", nr);
                    var i = 0;
                    if (ip < ir || ir == -1)
                        i = ip + 1;
                    else if (ip == ir && ir > -1)
                        i = ir + 2;
                    else if (ir > -1)
                        i = ir + 2;
                    splittedStringList.Add(s.Substring(nr, i - nr));
                    nr = i;
                }

                if (!string.IsNullOrEmpty(s.Substring(nr, s.Length - nr)))
                    splittedStringList.Add(s.Substring(nr, s.Length - nr));
            }

            return splittedStringList;
        }
    }

    public static class DcDictionary
    {
        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> @dic, TKey key, TValue value)
        {
            if (@dic == null) @dic = new Dictionary<TKey, TValue> { { key, value } };
            if (@dic.ContainsKey(key)) @dic[key] = value;
            else @dic.Add(key, value);
        }

        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> @dic, IDictionary<TKey, TValue> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
                @dic.Add<TKey, TValue>(keyValuePair.Key, keyValuePair.Value);
        }

        public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> @dic)
        {
            if (@dic == null) return true;
            return !@dic.Any();
        }
    }

    public class DcType<T> : DcFrameworkBase
    {
        public static T Formatteer(T @t)
        {
            AddLogging(@t);
            return @t;
        }

        public static T ParseWaarde(string waarde)
        {
            if (string.IsNullOrEmpty(waarde))
                return default(T);

            if (typeof(T) == typeof(DateTime))
                return (dynamic)DateTime.Parse(waarde, CultureInfo.InvariantCulture);

            if (typeof(T) == typeof(string))
                return (dynamic)waarde;

            var underlyingType = Nullable.GetUnderlyingType(typeof(T))
                ?? typeof(T);

            var safeValue = string.IsNullOrEmpty(waarde) ? null : Convert.ChangeType(waarde, underlyingType);

            AddLogging("from: '{waarde}' => to: '{safeValue}'".Formatteer(() => new[] {waarde, safeValue}), safeValue);
            
            return (T)safeValue;
        }
    }

    public class MemberInfoGetting : DcFrameworkBase
    {
        public static Dictionary<string, object> GetPropertyNameAndValue<T>(Expression<Func<T>> memberExpression)
        {
            var propertyNameAndValueDictionary = new Dictionary<string, object>();

            var ext = memberExpression.Body as NewArrayExpression;

            if (ext != null)
            {
                foreach (var memberEx in ext.Expressions)
                {
                    var kvp = GetMemberName(Expression.Lambda<Func<object>>(memberEx));
                    propertyNameAndValueDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            return propertyNameAndValueDictionary;
        }

        public static KeyValuePair<string, object> GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            if (memberExpression.Type.IsArray)
            {
                var ext = memberExpression.Body as NewArrayExpression;
                if (ext != null)
                {
                    foreach (var memberEx in ext.Expressions)
                    {
                        return GetMemberName(Expression.Lambda<Func<object>>(memberEx));
                    }
                }
            }
            else if (memberExpression.Body is ConstantExpression)
            {
                AddLogging("Property is a constant!", ((ConstantExpression) memberExpression.Body).Value);
            }
            else
            {
                var member = (MemberExpression)memberExpression.Body;
                var propertyName = member.Member.Name;
                T value = memberExpression.Compile()();
                AddLogging(string.Format("propertyName: {0} - value: {1}", propertyName, value));

                return new KeyValuePair<string, object>(propertyName, value);
            }
            return new KeyValuePair<string, object>();
        }
    }

    public static class DcControl
    {
        public static void SetFullWidthToAllControls(this Control.ControlCollection controlCollection)
        {
            var ownerControl = controlCollection.Owner;
            var maxWidth = ownerControl.Parent != null ? ownerControl.Parent.Width : (int?)null;
            if (!maxWidth.HasValue) return;

            maxWidth = maxWidth - ownerControl.Margin.Horizontal - ownerControl.Parent.Margin.Horizontal;

            controlCollection.Owner.Width = maxWidth.Value;
            foreach (var control in controlCollection.Cast<Control>())
            {
                control.Width = maxWidth.Value - control.Margin.Horizontal;
            }
        }
        public static void AddControl(this Control.ControlCollection controlCollection, Control controlToAdd)
        {
            var combinedHeight = 0;
            foreach (var control in controlCollection.Cast<Control>())
            {
                combinedHeight += control.Height;
            }
            controlToAdd.Location = new Point(controlToAdd.Location.X, combinedHeight);
            controlCollection.Add(controlToAdd);
        }
    }

    public class DcResource
    {
        public class Res
        {
            public string TypeName { get; set; }
            public string OriginalResourcePath { get; set; }
            public bool OriginalResourceFileExists { get { return File.Exists(OriginalResourcePath); } }
            public string NewResourcePath { get; set; }
            public bool NewResourceFileExists(string language) { return File.Exists(string.Format(NewResourcePath, language.ToUpper())); }
        }

        private Collection<Res> GetAllResourceFilesWithinAssembly<T>(string baseFolder)
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            var strippedAssembly = assembly.ManifestModule.Name.Remove(assembly.ManifestModule.Name.Length - 4);
            var resourceCollection = new Collection<Res>();

            foreach (var manifestResourceName in assembly.GetManifestResourceNames())
            {
                var resourceName =
                    manifestResourceName.Remove(manifestResourceName.Length - 10)
                                        .Replace(string.Format("{0}.", strippedAssembly), string.Empty);
                resourceCollection.Add(new Res
                {
                    TypeName = resourceName,
                    OriginalResourcePath = GetFullResourcePath(baseFolder, strippedAssembly, resourceName),
                    NewResourcePath = GetFullResourcePathWithLanguage(baseFolder, strippedAssembly, resourceName)
                });
            }
            return resourceCollection;
        }

        private static string GetFullResourcePath(string baseFolder, string assembly, string resourceName)
        {
            return string.Format(@"{0}{1}\{2}.resx", baseFolder, assembly, resourceName.Replace(".", @"\"));
        }

        private static string GetFullResourcePathWithLanguage(string baseFolder, string assembly, string resourceName)
        {
            return string.Format(@"{0}{1}\{2}{3}", baseFolder, assembly, resourceName.Replace(".", @"\"), ".{0}.resx");
        }

        private static void CreateResourceFile(Res res, string toLanguage)
        {
            if (!res.OriginalResourceFileExists) return;
            var rrw = new ResXResourceReader(res.OriginalResourcePath);
            if (res.OriginalResourcePath.Contains("Properties"))
            {
                //rrw.BasePath = @"C:\AlureOntwikkelingDev12\Alure.WS.BL\Resources";
            }

            var fileStream = File.Open(string.Format(res.NewResourcePath, toLanguage.ToUpper()), FileMode.OpenOrCreate);
            using (var resXResourceWriter = new ResXResourceWriter(fileStream))
            {
                try
                {
                    foreach (var dictionary in rrw.OfType<DictionaryEntry>())
                    {
                        var translatedValue = string.Empty;
                        if (!string.IsNullOrEmpty(dictionary.Value.ToString()))
                        {
                            var splittedStringList = DcString.SplitString(dictionary.Value.ToString());
                            foreach (var @string in splittedStringList.Where(x => !string.IsNullOrEmpty(x)))
                            {
                                var trimmedString = @string.Trim();
                                trimmedString = TranslateClass.Translate(trimmedString, "nl", toLanguage.ToLower());
                                if (@string.EndsWith("\r"))
                                    trimmedString += Environment.NewLine;
                                translatedValue += trimmedString;
                            }
                        }
                        if (string.IsNullOrEmpty(translatedValue))
                            translatedValue = string.Format("-TODO-{0}", dictionary.Value);
                        resXResourceWriter.AddResource(dictionary.Key.ToString(), translatedValue);
                    }
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}