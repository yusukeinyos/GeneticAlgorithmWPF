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
        private readonly List<Gene> _genes = new List<Gene>();

        /// <summary> 染色体タイプ </summary>
        public ChromosomesType ChromosomesType { get; set; } = ChromosomesType.Binary;

        /// <summary> 選択タイプ </summary>
        public SelectionType SelectionType { get; set; } = SelectionType.Roulette;

        /// <summary> 初期遺伝子数 </summary>
        public int InitialPopulationsNum { get; set; } = 100;

        /// <summary> 遺伝子長 </summary>
        public int GeneLength { get; set; }

        // 初期化
        public void Initialize(ChromosomesType chromosomes)
        {
            for (int i = 0; i < InitialPopulationsNum; i++)
            {
                _genes.Add(GAUtility.GenerateGene(ChromosomesType, GeneLength));
            }
        }

        /// <summary>
        /// すべての遺伝子の適用度の計算
        /// </summary>
        public void CalcAllFittness(Func<List<int>, double> fittnessFunc)
        {
            foreach (var gene in _genes)
            {
                gene.CalcFittness(x =>
                {
                    return x.Count(_ => _ > 0);
                });
            }
        }

        /// <summary>
        /// 選択
        /// </summary>
        /// <returns>選択された遺伝子のインデックス</returns>
        public int Selection(SelectionType selectionType)
        {
            var fittnessValues = _genes.Select(x => x.Fittness).ToArray();
            switch (selectionType)
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
        /// 1点交叉
        /// </summary>
        public void SinglePointCrossOver(Gene parent1, Gene parent2)
        {
            if (parent1.Chromosomes.Count != parent2.Chromosomes.Count)
            {
                Console.WriteLine("親の遺伝子の長さが違います。");
                return;
            }

            // 交叉点
            Random rand = new Random();
            var crossOverPoint = rand.Next(parent1.Chromosomes.Count);

            var chromosome1 = parent1.Chromosomes
                .Take(crossOverPoint);

            var chromosome2 = parent2.Chromosomes
                .Take(crossOverPoint);

            _genes.Add(new Gene
            {
                Chromosomes = chromosome1.Concat(parent2.Chromosomes
                    .Skip(crossOverPoint)
                    .Take(parent2.Chromosomes.Count - crossOverPoint))
                    .ToList(),
                GenerationNum = parent1.GenerationNum + 1,
            });
            _genes.Add(new Gene
            {
                Chromosomes = chromosome2.Concat(parent1.Chromosomes
                    .Skip(crossOverPoint)
                    .Take(parent1.Chromosomes.Count - crossOverPoint))
                    .ToList(),
                GenerationNum = parent2.GenerationNum + 1,
            });
        }
    }
}
