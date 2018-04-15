using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmWPF.Utility
{
    public static class RandomProvider
    {
        public static Random Random => _random ?? (_random = new Random());
        private static Random _random;

        public static int NextInt(int upper) => Random.Next(upper);

        public static double NextDouble() => Random.NextDouble();

        public static bool DrawLots(float rate, float maxRate) => rate >= maxRate ||
                                                                  rate > 0 && Random.Next((int)maxRate + 1) <= rate;
    }
}
