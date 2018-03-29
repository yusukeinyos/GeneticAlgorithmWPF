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
    }
}
