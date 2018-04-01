using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmWPF.TravellingSalesmanProblem.Model;

namespace GeneticAlgorithmWPF.TravellingSalesmanProblem.Utility
{
    public static class ModelUtility
    {
        // 全ての都市データを取得します
        public static IEnumerable<City> GetAllCitiesModel(List<string[]> datas)
        {
            foreach (var data in datas)
            {
                yield return new City
                {
                    Id = int.Parse(data[0]),
                    Name = data[1],
                    X = double.Parse(data[2]),
                    Y = double.Parse(data[3]),
                };
            }
        }
    }
}
