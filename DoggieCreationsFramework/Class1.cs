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

        public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> @dic)
        {
            if (@dic == null) return true;
            return !@dic.Any();
        }
    }

    public class DcType<T> : FrameworkBase
    {
        public static KeyValuePair<T, ApplicationException> Formatteer(T @t)
        {

            AddLogging();
            return new KeyValuePair<T, ApplicationException>(@t, null);
        }
    }

    public class FrameworkBase
    {
        private static Dictionary<LoggerType, IDoggieCreationsLogger> _loggers; 
        public static Dictionary<LoggerType, IDoggieCreationsLogger> Loggers
        {
            get
            {
                if (_loggers.IsEmpty())
                {
                    _loggers.Add(new KeyValuePair<LoggerType, IDoggieCreationsLogger>(LoggerType.UnitTest, new DoggieCreationsUnitTestLogger()));
                    _loggers.Add(LoggerType.Live, new DoggieCreationsLogger());
                }
                return _loggers;
            }
        }

        public static void AddLogging()
        {
            Loggers[LoggerType.UnitTest].AddLogging();
        }
    }

    public enum LoggerType
    {
        UnitTest,
        Live
    }

    public class DoggieCreationsLogger : IDoggieCreationsLogger
    {
        public Collection<IDoggieCreationsLogger> Loggers;

        public static ApplicationException LogShit<T>(string message)
        {
            return new ApplicationException(message);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void AddLogging()
        {
            throw new NotImplementedException();
        }
    }

    public class DoggieCreationsUnitTestLogger : IDoggieCreationsLogger
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void AddLogging()
        {
            throw new NotImplementedException();
        }
    }

    public interface IDoggieCreationsLogger : IDisposable
    {
        void AddLogging();
    }
}
