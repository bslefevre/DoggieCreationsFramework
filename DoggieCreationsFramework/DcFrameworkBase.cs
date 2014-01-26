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

        private static LoggerType? _activeLogger;
        protected static LoggerType ActiveLogger
        {
            get
            {
                if (!_activeLogger.HasValue) return LoggerType.UnitTest;
                return _activeLogger.Value;
            }
        }

        public static IDoggieCreationsLogger Logging
        {
            get { return Loggers[ActiveLogger]; }
        }

        public static LoggerType SetLoggingType
        {
            set { _activeLogger = value; }
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