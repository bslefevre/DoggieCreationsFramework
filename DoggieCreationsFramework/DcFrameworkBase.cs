using System.Collections.Generic;

namespace DoggieCreationsFramework
{
    public class DcFrameworkBase
    {
        private static Dictionary<LoggerType, IDoggieCreationsLogger> _loggers;
        protected static Dictionary<LoggerType, IDoggieCreationsLogger> Loggers
        {
            get
            {
                if (_loggers.IsEmpty())
                {
                    _loggers = new Dictionary<LoggerType, IDoggieCreationsLogger>
                        {
                            {LoggerType.UnitTest, new DoggieCreationsUnitTestLogger()},
                            {LoggerType.Live, new DoggieCreationsLogger()}
                        };
                }
                return _loggers;
            }
        }

        public static IDoggieCreationsLogger Logging
        {
            get { return Loggers[LoggerType.UnitTest]; }
        }

        public static void AddLogging(object t)
        {
            Loggers[LoggerType.UnitTest].AddLogging(t);
        }

        public enum LoggerType
        {
            UnitTest,
            Live
        }
    }
}