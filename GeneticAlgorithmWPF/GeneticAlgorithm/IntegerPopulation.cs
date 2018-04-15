using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmWPF.GeneticAlgorithm.Utility;
using GeneticAlgorithmWPF.Utility;

namespace GeneticAlgorithmWPF.GeneticAlgorithm
{
    public interface IPopulation
    {
        void Initialize();
        void ClearGenes();
        void CalcAllFittness();
        int Selection();
        
    }

    public abstract class PopulationBase<T> : IPopulation
    {
        public abstract void CalcAllFittness();

        public abstract void ClearGenes();

        public abstract List<T>[] CrossOver();

        public abstract void Initialize();

        public abstract int Selection();
    }

    /// <summary>
    /// 遺伝子プールクラス
    /// </summary>
    public class IntegerPopulation : PopulationBase<int>
    {
        /// <summary> 遺伝子リスト </summary>
        private readonly List<IGene<int>> _genes = new List<IGene<int>>();

        /// <summary> 染色体タイプ </summary>
        public ChromosomesType ChromosomesType { get; set; } = ChromosomesType.Binary;

        /// <summary> 選択タイプ </summary>
        public SelectionType SelectionType { get; set; } = SelectionType.Roulette;

        /// <summary> 交叉タイプ </summary>
        public CrossOverType CrossOverType { get; set; } = CrossOverType.SinglePoint;

        /// <summary> 初期遺伝子数 </summary>
        public int InitialPopulationSize { get; set; } = 100;

        /// <summary> 世代数 </summary>
        public int GenerationNum { get; set; }

        // TODO: いらないかも
        public int PopulationSize => _populationSize;

        public Func<List<int>, double> CalcFunc { get; set; }


        /// <summary> 遺伝子長 </summary>
        public int GeneLength { get; set; }

        /// <summary> 遺伝子数 </summary>
        private int _populationSize;
        

        // 初期化
        public override void Initialize()
        {
            for (int i = 0; i < InitialPopulationSize; i++)
            {
                _genes.Add(GAUtility.GenerateGene(ChromosomesType, CalcFunc, GeneLength));
                _populationSize++;
            }
        }

        // 遺伝子を追加します
        public void AddGene(List<int> chromosomes, Func<List<int>, double> calcFunc)
        {
            _genes.Add(new IntegerGene
            {
                Chromosomes = chromosomes,
                GenerationNum = GenerationNum,
                CalcFunc = calcFunc,
                Fittness = 0,
            });
            _populationSize++;
        }

        // 初期個体数まで個体を淘汰します
        public void DecreasePopulation()
        {
            var decreaseNum = _genes.Count > InitialPopulationSize ? _genes.Count - InitialPopulationSize : 0;

            // 適用度が小さい順にソート
            _genes.Sort((x, y) => (int)(y.Fittness - x.Fittness));
            // 淘汰
            _genes.RemoveRange(0, decreaseNum);
        }

        /// <summary>
        /// 遺伝子のクリア
        /// </summary>
        public override void ClearGenes()
        {
            _genes.Clear();
            _populationSize = 0;
        }

        /// <summary>
        /// すべての遺伝子の適用度の計算
        /// </summary>
        public override void CalcAllFittness()
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
        public override int Selection()
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
        public override List<int>[] CrossOver()
        {
            var targetIndeces = Enumerable.Range(0, _genes.Count).Shuffle().Take(2).ToArray();
            switch (CrossOverType)
            {
                case CrossOverType.SinglePoint:
                    return GAUtility.SinglePointCrossOver(_genes[targetIndeces[0]].Chromosomes, _genes[targetIndeces[1]].Chromosomes);
                                          
                case CrossOverType.DoublePoint:
                    return null;
                case CrossOverType.PermutationSinglePoint:
                    return GAUtility.PermutationCrossOver(_genes[targetIndeces[0]].Chromosomes, _genes[targetIndeces[1]].Chromosomes, isSingleCross: true);
                case CrossOverType.PermutationDoublePoint:
                    return GAUtility.PermutationCrossOver(_genes[targetIndeces[0]].Chromosomes, _genes[targetIndeces[1]].Chromosomes, isSingleCross: false);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 全ての適用度を取得します
        /// </summary>
        public List<double> GetFittnessAll()
        {
            return _genes.Select(x => x.Fittness).ToList();
        }

        /// <summary>
        /// 適用度の平均を返します
        /// </summary>
        public double GetAverageFittness()
        {
            return _genes.Average(x => x.Fittness);
        }

        /// <summary>
        /// 適用度の最大値を返します
        /// </summary>
        public double GetTopFittness()
        {
            return GetTopGene().Fittness;
        }

        /// <summary>
        /// 適用度が最大の個体を取得します
        /// </summary>
        public IGene<int> GetTopGene()
        {
            return _genes.OrderBy(x => x.Fittness).First();
        }

        /// <summary>
        /// 指定したインデックスの個体を返します
        /// </summary>
        public IGene<int> GetGeneAt(int index)
        {
            return _genes[index];
        }
    }
}
