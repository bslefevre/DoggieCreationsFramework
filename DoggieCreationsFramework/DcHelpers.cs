using System.Collections.Generic;
using System.Linq;

namespace DoggieCreationsFramework
{
    public static class DcString
    {
        public static string Formatteer(this string @string)
        {
            return DcType<string>.Formatteer(@string);
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
    }
}