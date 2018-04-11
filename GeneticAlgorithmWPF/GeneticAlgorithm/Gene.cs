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

    public interface IGene
    {
        int GenerationNum { get; set; }
        double Fittness { get; set; }

        void CalcFittness();
    }

    public abstract class GeneBase<T> : IGene
    {
        public abstract List<T> Chromosomes { get; set; }
        public abstract int GenerationNum { get; set; }
        public abstract double Fittness { get; set; }
        public abstract Func<List<T>, double> CalcFunc { get; set; }

        /// <summary>
        /// 適用度を計算します
        /// </summary>
        public void CalcFittness()
        {
            Fittness = CalcFunc(Chromosomes);
        }
    }

    /// <summary>
    /// 遺伝子クラス
    /// </summary>
    public class Gene : GeneBase<int>
    {
        public override List<int> Chromosomes { get; set; }
        public override int GenerationNum { get; set; }
        public override double Fittness { get; set; }
        public override Func<List<int>, double> CalcFunc { get; set; }
    }
}
