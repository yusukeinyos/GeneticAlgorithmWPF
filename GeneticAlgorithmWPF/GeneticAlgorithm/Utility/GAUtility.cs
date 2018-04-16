using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmWPF.Utility;

namespace GeneticAlgorithmWPF.GeneticAlgorithm.Utility
{
    /// <summary>
    /// 選択タイプ
    /// </summary>
    public enum SelectionType : byte
    {
        Roulette,
        Rank,
    }

    /// <summary>
    /// 交叉タイプ
    /// </summary>
    public enum CrossOverType : byte
    {
        SinglePoint,
        DoublePoint,
        PermutationSinglePoint,
        PermutationDoublePoint,
    }

    /// <summary>
    /// 突然変異タイプ
    /// </summary>
    public enum MutationType : byte
    {
        Swap,
    }

    public static class GAUtility
    {
        #region 生成

        /// <summary>
        /// 遺伝子を生成します
        /// </summary>
        /// <param name="chromosomesType"></param>
        /// <returns></returns>
        public static IntegerGene GenerateGene(ChromosomesType chromosomesType, Func<List<int>, double> calcFunc, int geneLength)
        {
            var gene = new IntegerGene
            {
                GenerationNum = 0,
                CalcFunc = calcFunc,
                Fittness = 0,
            };
            switch (chromosomesType)
            {
                case ChromosomesType.Binary:
                    gene.Chromosomes = GenerateBinaryChromosomes(geneLength);
                    break;
                case ChromosomesType.Permutaion:
                    gene.Chromosomes = GeneratePermutationChromosomes(geneLength);
                    break;
            }

            return gene;
        }

        /// <summary>
        /// バイナリの染色体を返します
        /// </summary>
        private static List<int> GenerateBinaryChromosomes(int geneLength)
        {
            var chromosomes = new List<int>();
            for (int i = 0; i < geneLength; i++)
            {
                chromosomes.Add(
                    RandomProvider.NextInt(100) % 2 == 0
                        ? 0
                        : 1
                );
            }

            return chromosomes;
        }

        /// <summary>
        /// 順列の染色体を返します
        /// </summary>
        private static List<int> GeneratePermutationChromosomes(int geneLength)
        {
            return Enumerable.Range(0, geneLength).Shuffle().ToList();
        }

        #endregion

        #region 選択

        /// <summary>
        /// ルーレット法による選択
        /// </summary>
        /// <param name="fittnessValues">適用度のリスト</param>
        /// <returns>選択された遺伝子のインデックス</returns>
        public static int RouletteSelection(double[] fittnessValues)
        {
            // 規格化
            var sumOfValues = fittnessValues.Sum();
            var rouletteValues = fittnessValues.Select(x => x / sumOfValues).ToArray();

            // ルーレット値の生成
            for (int i = 1; i < rouletteValues.Length; i++)
            {
                rouletteValues[i] += rouletteValues[i - 1];
            }

            var selected = RandomProvider.NextDouble();
            return Array.FindIndex(rouletteValues, x => x > selected);
        }

        /// <summary>
        /// ランク法による選択
        /// </summary>
        /// <param name="fittnessValues">適用度のリスト</param>
        /// <returns>選択された遺伝子のインデックス</returns>
        public static int RankSelection(double[] fittnessValues)
        {
            var selectedIndex = RouletteSelection(
                Enumerable.Range(1, fittnessValues.Length).Reverse().Select(x => (double)x).ToArray());
            return fittnessValues.Select((v, i) => new { Value = v, Index = i })
                .OrderBy(x => x.Value).ToArray()[selectedIndex].Index;

        }

        #endregion

        #region 交叉

        /// <summary>
        /// 順列表現の交叉
        /// </summary>
        public static List<T>[] PermutationCrossOver<T>(List<T> chromosomes1, List<T> chromosomes2, bool isSingleCross)
        {
            var length1 = chromosomes1.Count;
            var length2 = chromosomes2.Count;
            if (length1 != length2)
            {
                Console.WriteLine("親の遺伝子サイズが異なります。");
                return null;
            }

            var toCrossOverPoint = RandomProvider.NextInt(length1);
            var fromCrossOverPoint = isSingleCross ? 0 : RandomProvider.NextInt(toCrossOverPoint);
            var crossPart = chromosomes1.Skip(fromCrossOverPoint).Take(toCrossOverPoint - fromCrossOverPoint + 1).ToArray();

            var offspringChromosomes = new List<T>(length1);
            var j = 0;
            for (int i = 0; i < length1 || j < length2; i++)
            {
                if (i >= fromCrossOverPoint && i <= toCrossOverPoint)
                {
                    offspringChromosomes.Add(chromosomes1[i]);
                }
                else
                {
                    if (!crossPart.Contains(chromosomes2[j]))
                    {
                        offspringChromosomes.Add(chromosomes2[j++]);
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            return new[] { offspringChromosomes };
        }

        /// <summary>
        /// 1点交叉
        /// </summary>
        public static List<T>[] SinglePointCrossOver<T>(List<T> chromosomes1, List<T> chromosomes2)
        {
            if (chromosomes1.Count != chromosomes2.Count)
            {
                Console.WriteLine("親の遺伝子の長さが違います。");
                return null;
            }

            // 交叉点
            var crossOverPoint = RandomProvider.NextInt(chromosomes1.Count);

            var part1 = chromosomes1
                .Take(crossOverPoint);

            var part2 = chromosomes2
                .Take(crossOverPoint);

            return new List<T>[]
            {
                part1.Concat(chromosomes2
                    .Skip(crossOverPoint)
                    .Take(chromosomes2.Count - crossOverPoint))
                    .ToList(),
                part2.Concat(chromosomes1
                    .Skip(crossOverPoint)
                    .Take(chromosomes1.Count - crossOverPoint))
                    .ToList(),
            };
        }

        #endregion

        #region 突然変異

        /// <summary>
        /// スワップ系
        /// </summary>
        public static List<T> SwapMutation<T>(List<T> chromosomes, float mutationRate)
        {
            if (!RandomProvider.DrawLots(mutationRate, 100))
                return chromosomes;

            var count = chromosomes.Count;
            var swapIndex = Enumerable.Range(0, count).Shuffle().Take(2).ToArray();

            var temp = chromosomes[swapIndex[0]];
            chromosomes[swapIndex[0]] = chromosomes[swapIndex[1]];
            chromosomes[swapIndex[1]] = temp;

            return chromosomes;
        }

        #endregion
    }


}

