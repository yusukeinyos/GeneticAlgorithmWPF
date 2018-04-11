using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmWPF.GeneticAlgorithm.Utility;

namespace GeneticAlgorithmWPF.GeneticAlgorithm
{
    /// <summary>
    /// 遺伝子プールクラス
    /// </summary>
    public class Population
    {
        /// <summary> 遺伝子リスト </summary>
        private readonly List<IGene> _genes = new List<IGene>();

        /// <summary> 染色体タイプ </summary>
        public ChromosomesType ChromosomesType { get; set; } = ChromosomesType.Binary;

        /// <summary> 選択タイプ </summary>
        public SelectionType SelectionType { get; set; } = SelectionType.Roulette;

        /// <summary> 交叉タイプ </summary>
        public CrossOverType CrossOverType { get; set; } = CrossOverType.SinglePoint;

        /// <summary> 初期遺伝子数 </summary>
        public int InitialPopulationSize { get; set; } = 100;

        /// <summary> 遺伝子長 </summary>
        public int GeneLength { get; set; }

        // 初期化
        public void Initialize()
        {
            for (int i = 0; i < InitialPopulationSize; i++)
            {
                _genes.Add(GAUtility.GenerateGene(ChromosomesType, GeneLength));
            }
        }

        /// <summary>
        /// すべての遺伝子の適用度の計算
        /// </summary>
        public void CalcAllFittness()
        {
            foreach (var gene in _genes)
            {
                gene.CalcFittness();
            }
        }

        /// <summary>
        /// 選択
        /// </summary>
        /// <returns>選択された遺伝子のインデックス</returns>
        public int Selection()
        {
            var fittnessValues = _genes.Select(x => x.Fittness).ToArray();
            switch (SelectionType)
            {
                case SelectionType.Roulette:
                    return GAUtility.RouletteSelection(fittnessValues);
                case SelectionType.Rank:
                    return GAUtility.RankSelection(fittnessValues);
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// 交叉
        /// </summary>
        public void CrossOver(int[] selectedIndex)
        {
            switch (CrossOverType)
            {
                case CrossOverType.SinglePoint:
                    break;
                case CrossOverType.DoublePoint:
                    break;
                case CrossOverType.Permutation:
                    break;
            }
        }

    }
}
