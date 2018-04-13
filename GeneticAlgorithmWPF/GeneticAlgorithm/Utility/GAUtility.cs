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
        Permutation,
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
        public static Gene GenerateGene(ChromosomesType chromosomesType, int geneLength)
        {
            var gene = new Gene
            {
                GenerationNum = 0,
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
            Random rand = new Random();
            var chromosomes = new List<int>();
            for (int i = 0; i < geneLength; i++)
            {
                chromosomes.Add(
                    rand.Next(100) % 2 == 0
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

            Random rand = new Random();
            return Array.FindIndex(rouletteValues, x => x > rand.NextDouble());
        }

        /// <summary>
        /// ランク法による選択
        /// </summary>
        /// <param name="fittnessValues">適用度のリスト</param>
        /// <returns>選択された遺伝子のインデックス</returns>
        public static int RankSelection(double[] fittnessValues)
        {
            var selectedIndex = RouletteSelection(
                Enumerable.Range(1, fittnessValues.Length + 1).Select(x => (double)x).ToArray());
            return fittnessValues.Select((v, i) => new { Value = v, Index = i })
                .OrderBy(x => x.Value).ToArray()[selectedIndex].Index;

        }

        #endregion

        #region 交叉

        /// <summary>
        /// 順列表現の交叉
        /// </summary>
        public static Gene PermutationCrossOver(Gene parent1, Gene parent2)
        {
            var chromosomes1 = parent1.Chromosomes;
            var chromosomes2 = parent2.Chromosomes;

            var length1 = chromosomes1.Count;
            var length2 = chromosomes2.Count;
            if (length1 != length2)
            {
                Console.WriteLine("親の遺伝子サイズが異なります。");
                return null;
            }

            Random rand = new Random();
            var toCrossOverPoint = rand.Next(length1);
            var fromCrossOverPoint = rand.Next(toCrossOverPoint);
            var crossPart = chromosomes1.Skip(toCrossOverPoint).Take(toCrossOverPoint - fromCrossOverPoint + 1).ToArray();

            var offspringChromosomes = new List<int>(length1);
            var j = 0;
            for (int i = 0; i < length2; i++)
            {
                if (i >= fromCrossOverPoint && i <= toCrossOverPoint)
                {
                    offspringChromosomes[i] = chromosomes1[i];
                }
                else
                {
                    if (!crossPart.Contains(chromosomes2[j]))
                    {
                        offspringChromosomes[i] = chromosomes2[j++];
                    }
                    else
                    {
                        j++;
                    }
                }
            }

            return new Gene
            {
                Chromosomes = offspringChromosomes,
                GenerationNum = parent1.GenerationNum++,
            };
        }

        /// <summary>
        /// 1点交叉
        /// </summary>
        public static Gene[] SinglePointCrossOver(Gene parent1, Gene parent2)
        {
            if (parent1.Chromosomes.Count != parent2.Chromosomes.Count)
            {
                Console.WriteLine("親の遺伝子の長さが違います。");
                return null;
            }

            // 交叉点
            Random rand = new Random();
            var crossOverPoint = rand.Next(parent1.Chromosomes.Count);

            var chromosome1 = parent1.Chromosomes
                .Take(crossOverPoint);

            var chromosome2 = parent2.Chromosomes
                .Take(crossOverPoint);

            return new Gene[]
            {
                new Gene
                {
                    Chromosomes = chromosome1.Concat(parent2.Chromosomes
                            .Skip(crossOverPoint)
                            .Take(parent2.Chromosomes.Count - crossOverPoint))
                        .ToList(),
                    GenerationNum = parent1.GenerationNum + 1,
                },
                new Gene
                {
                    Chromosomes = chromosome2.Concat(parent1.Chromosomes
                            .Skip(crossOverPoint)
                            .Take(parent1.Chromosomes.Count - crossOverPoint))
                        .ToList(),
                    GenerationNum = parent2.GenerationNum + 1,
                }
            };
        }

        #region 突然変異

        /// <summary>
        /// スワップ系
        /// </summary>
        public static List<T> SwapMutation<T>(List<T> chromosomes, float mutationRate)
        {
            if (RandomProvider.DrawLots(mutationRate, 100))
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

