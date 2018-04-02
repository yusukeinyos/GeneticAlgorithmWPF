using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmWPF.GeneticAlgorithm.Utility
{
    public static class GAUtility
    {
        /// <summary>
        /// 順列表現の交叉
        /// </summary>
        public static int[] PermutationCrossOver(int[] parent1, int[] parent2)
        {
            var length1 = parent1.Length;
            var length2 = parent2.Length;
            if (length1 != length2)
            {
                Console.WriteLine("親の遺伝子サイズが異なります。");
                return Array.Empty<int>();
            }

            Random rand = new Random();
            var toCrossOverPoint = rand.Next(length1);
            var fromCrossOverPoint = rand.Next(toCrossOverPoint);
            var crossPart = parent1.Skip(toCrossOverPoint).Take(toCrossOverPoint - fromCrossOverPoint + 1).ToArray();

            var offspring = new int[length1];
            var j = 0;
            for (int i = 0; i < length2; i++)
            {
                if (i >= fromCrossOverPoint && i <= toCrossOverPoint)
                {
                    offspring[i] = parent1[i];
                }
                else
                {
                    if (!crossPart.Contains(parent2[j]))
                    {
                        offspring[i] = parent2[j++];
                    }
                    else
                    {
                        j++;
                    }
                }               
            }

            return offspring;
        }
    }
}
