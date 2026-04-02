using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Engine
{
    internal static class Logger
    {
        public static event Action<string> OnLog;

        public static void Log(string message)
        {
            OnLog?.Invoke(message);
        }

        public static void Clear()
        {
            OnLog?.Invoke(null);
        }
    }
}
