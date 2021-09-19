using System.Collections.Generic;

namespace SeaDog
{
    public class Log
    {
        private readonly List<string> _content = new List<string>();

        public Log() { }

        public void Add(string log)
        {
            _content.Add(log);
        }

        public string[] Logs => _content.ToArray();
        
    }
}
