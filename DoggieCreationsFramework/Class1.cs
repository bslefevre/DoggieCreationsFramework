using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DoggieCreationsFramework
{
    public static class DcString
    {
        public static string Formatteer(this string @string)
        {
            var keyValuePair = DcType<string>.Formatteer(@string);
            Logging.Add(keyValuePair);
            return keyValuePair.Key;
        }
    }

    public static class DcDictionary
    {
        public static void Add<TKey, TValue>(this Dictionary<TKey, TValue> @dic, KeyValuePair<TKey, TValue> keyValuePair)
        {
            if (@dic == null) @dic = new Dictionary<TKey, TValue> {{keyValuePair.Key, keyValuePair.Value}};
            if (@dic.ContainsKey(keyValuePair.Key)) @dic[keyValuePair.Key] = keyValuePair.Value;
            else @dic.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }

    public class DcType<T> : DoggieCreationsLogger<T>
    {
        public static KeyValuePair<T, ApplicationException> Formatteer(T @t)
        {
            return new KeyValuePair<T, ApplicationException>(@t, LogShit<T>("Damn"));
        }
    }

    public abstract class DoggieCreationsLogger<T> : IDoggieCreationsLogger<T>
    {
        public Collection<IDoggieCreationsLogger<T>> Loggers;

        public static ApplicationException LogShit<T>(string message)
        {
            return new ApplicationException(message);
        }

        public delegate void AddLogging(T @t);

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class DoggieCreationsUnitTestLogger<T> : IDoggieCreationsLogger<T>
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public interface IDoggieCreationsLogger<T> : IDisposable
    {
        
    }
}
