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
    }

    public interface IDoggieCreationsLogger : IDisposable
    {
        void AddLogging(object @object);
    }

    public class Logging : Exception
    {
        private readonly object _object;
        public Logging(object @object)
        {
            _object = @object;
        }

        public override string Message
        {
            get
            {
                return string.Format("{0}: {1}", _object, _object.GetType());
            }
        }
    }
}