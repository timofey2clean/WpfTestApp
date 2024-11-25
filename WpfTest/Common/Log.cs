using System;
using System.Text;

namespace WpfTest.Common
{
    class Log
    {
        private static Log _inst;
        private static Object _locker = new Object();

        private readonly StringBuilder _strBuilder;

        private Log()
        {            
            _strBuilder = new StringBuilder();
        }

        public static Log Inst()
        {
            if (ReferenceEquals(null, _inst))
            {
                lock (_locker)
                {
                    if (ReferenceEquals(null, _inst))
                    {
                        _inst = new Log();
                    }
                }
            }

            return _inst;
        }

        public void Exception(Exception ex)
        {
            lock(_locker)
                _strBuilder.AppendFormat("[{0}][error] {1}{2}", DateTime.Now.ToLongTimeString(), ex.Message, Environment.NewLine);
        }

        public void Message(String text, params Object[] args)
        {
            lock (_locker)
                _strBuilder.AppendFormat("[{0}] {1}{2}", DateTime.Now.ToLongTimeString(), String.Format(text, args), Environment.NewLine);
        }

        public String GetText()
        {
            lock (_locker)
                return _strBuilder.ToString();
        }
    }
}
