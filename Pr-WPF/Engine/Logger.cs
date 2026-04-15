using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Engine
{
    public static class Logger
    {
        private static StringBuilder _history = new StringBuilder();
        public static event Action<string> OnLog;

        public static string FullLog => _history.ToString();

        public static void Log(string message)
        {
            _history.AppendLine(message);
            OnLog?.Invoke(message);
        }

        public static void Clear()
        {
            _history.Clear();
            OnLog?.Invoke(null);
        }
    }
}
