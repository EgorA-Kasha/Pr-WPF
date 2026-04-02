using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pr_WPF.Engine
{
    internal static class Rng
    {
        private static readonly Random _random = new Random();

        public static int Next(int min, int max) => _random.Next(min, max);
        public static double NextDouble() => _random.NextDouble();
        public static double NextDouble(double min, double max) => min + (_random.NextDouble() * (max - min));
    }
}
