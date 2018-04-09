using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmWPF.GeneticAlgorithm
{
    /// <summary>
    /// 染色体のタイプ
    /// </summary>
    public enum ChromosomesType
    {
        Binary,
        Permutaion,
    }

    /// <summary>
    /// 遺伝子クラス
    /// </summary>
    public class Gene
    {
        public List<int> Chromosomes { get; set; }
        public int GenerationNum { get; set; }
        public double Fittness { get; set; }

        /// <summary>
        /// 適用度の計算
        /// </summary>
        public void CalcFittness(Func<List<int>, double> fittnessFunc)
        {
            Fittness = fittnessFunc(Chromosomes);
        }
    }
}
