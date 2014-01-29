using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DoggieCreationsFramework
{
    public class DoggieCreationsLogger : IDoggieCreationsLogger
    {
        public Collection<IDoggieCreationsLogger> Loggers;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void AddLogging(object @object)
        {
            throw new NotImplementedException();
        }

        public void AddLogging(string message, object @object)
        {
            throw new NotImplementedException();
        }
    }

    public class DoggieCreationsUnitTestLogger : IDoggieCreationsLogger
    {
        private Dictionary<object, Logging> _logging;
        public Dictionary<object, Logging> Logging
        {
            get
            {
                if (_logging.IsEmpty()) { _logging = new Dictionary<object, Logging>(); }
                return _logging;
            }
            set { _logging = value; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void AddLogging(object @object)
        {
            Logging.Add<object, Logging>(@object, new Logging(@object));
        }

        public void AddLogging(string message, object @object)
        {
            Logging.Add<object, Logging>(@object, new Logging(message, @object));
        }
    }

    public interface IDoggieCreationsLogger : IDisposable
    {
        void AddLogging(object @object);
        void AddLogging(string message, object @object);
    }

    public class Logging : Exception
    {
        private readonly object _object;
        private readonly string _message;

        public Logging(object @object)
        {
            _object = @object;
            _message = string.Format("{0}: {1}", _object, _object.GetType());
        }

        public Logging(string message, object @object)
        {
            _message = message;
            _object = @object;
        }

        public override string Message
        {
            get { return _message; }
        }
    }
}