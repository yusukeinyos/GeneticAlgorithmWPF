using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmWPF.Utility
{
    public static class RandomProvider
    {
        public static Random Random => new Random();
        public static bool DrawLots(float rate, float maxRate) => rate >= maxRate ||
                                                                  rate > 0 && Random.Next((int)maxRate + 1) <= rate;
    }
}
