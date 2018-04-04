using GeneticAlgorithmWPF.TravellingSalesmanProblem.Model;
using System;

namespace GeneticAlgorithmWPF.TravellingSalesmanProblem.Utility
{
    public static class CityUtility
    {
        /// <summary>
        /// ２都市間の距離を返します
        /// </summary>
        public static double GetDistance(City city1, City city2)
        {
            var x1minus2 = city1.X - city2.X;
            var y1minus2 = city1.Y - city2.Y;
            return Math.Sqrt(x1minus2 * x1minus2 + y1minus2 * y1minus2);
        }

        /// <summary>
        /// パスの全コストを返します
        /// </summary>
        public static double GetAllPathCost(City[] cityPath)
        {
            double allCost = 0;
            var pathLength = cityPath.Length;
            for (int i = 0; i < pathLength; i++)
            {
                allCost += GetDistance(cityPath[i], cityPath[i < pathLength - 1 ? i + 1 : 0]);
            }

            return allCost;
        }
    }
}
