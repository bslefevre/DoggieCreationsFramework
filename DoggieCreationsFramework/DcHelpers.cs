using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;

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

        public static string Formatteer(this string @string, Expression<Func<string[]>> expression)
        {
            var propertyNameAndValueDictionary = MemberInfoGetting.GetPropertyNameAndValue<string[]>(expression);

            foreach (var kvp in propertyNameAndValueDictionary)
            {
                @string = @string.Replace(string.Format("{{{0}}}", kvp.Key), kvp.Value.ToString());
            }

            return @string;
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
                    var kvp = GetMemberName<object>(Expression.Lambda<Func<object>>(memberEx));
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
                        return GetMemberName<object>(Expression.Lambda<Func<object>>(memberEx));
                    }
                }
            }
            else
            {
                var member = (MemberExpression)memberExpression.Body;
                string propertyName = member.Member.Name;
                T value = memberExpression.Compile()();
                AddLogging(string.Format("propertyName: {0} - value: {1}", propertyName, value));

                return new KeyValuePair<string, object>(propertyName, value);
            }
            return new KeyValuePair<string, object>();
        }
    }
}