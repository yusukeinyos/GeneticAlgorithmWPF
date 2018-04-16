using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmWPF.GeneticAlgorithm.Utility;

namespace GeneticAlgorithmWPF.GeneticAlgorithm
{
    public class GASolver
    {
        /// <summary>
        /// 内部クラス
        /// </summary>
        public class GASolverInfo
        {
            public readonly ChromosomesType ChromosomesType;
            public readonly SelectionType SelectionType;
            public readonly CrossOverType CrossOverType;
            public readonly MutationType MutationType;
            public readonly float MutationRate;
            public readonly int InitialPopulationSize;
            public readonly int GeneLength;
            public readonly Func<List<int>, double> CalcFunc;

            public GASolverInfo(ChromosomesType chromosomesType, SelectionType selectionType, CrossOverType crossOverType, MutationType mutationType, float mutationRate, int initialPopulationSize, int geneLength, Func<List<int>, double> calcFunc)
            {
                ChromosomesType = chromosomesType;
                SelectionType = selectionType;
                CrossOverType = crossOverType;
                MutationType = mutationType;
                MutationRate = mutationRate;
                InitialPopulationSize = initialPopulationSize;
                GeneLength = geneLength;
                CalcFunc = calcFunc;
            }
        }

        public int PopulationSize { get; set; }

        private GASolverInfo _solverInfo;
        private IntegerPopulation _currentPopulation;
        private IntegerPopulation _nextPopulation;

        #region メソッド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GASolver(GASolverInfo solverInfo)
        {
            _solverInfo = solverInfo;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            switch (_solverInfo.ChromosomesType)
            {
                case ChromosomesType.Binary:
                case ChromosomesType.Permutaion:
                    _currentPopulation = new IntegerPopulation
                    {
                        ChromosomesType = _solverInfo.ChromosomesType,
                        SelectionType = _solverInfo.SelectionType,
                        CrossOverType = _solverInfo.CrossOverType,
                        MutationType = _solverInfo.MutationType,
                        MutationRate = _solverInfo.MutationRate,
                        InitialPopulationSize = _solverInfo.InitialPopulationSize,
                        GeneLength = _solverInfo.GeneLength,
                        CalcFunc = _solverInfo.CalcFunc,
                    };
                    break;
            }


            _currentPopulation.Initialize();
        }

        public void SolveAll()
        {

        }

        public void SolveOneStep()
        {
            _nextPopulation = new IntegerPopulation
            {
                ChromosomesType = _solverInfo.ChromosomesType,
                SelectionType = _solverInfo.SelectionType,
                CrossOverType = _solverInfo.CrossOverType,
                MutationType = _solverInfo.MutationType,
                MutationRate = _solverInfo.MutationRate,
                InitialPopulationSize = _solverInfo.InitialPopulationSize,
                GeneLength = _solverInfo.GeneLength,
                GenerationNum = _currentPopulation.GenerationNum + 1,
            };

            // 適用度の計算
            _currentPopulation.CalcAllFittness();

            // 選択
            for (int j = 0; j < _solverInfo.InitialPopulationSize; j++)
            {
                _nextPopulation.AddGene(_currentPopulation.GetGeneAt(_currentPopulation.Selection()).Chromosomes, _solverInfo.CalcFunc);
            }

            for (int i = 0; i < _solverInfo.InitialPopulationSize; i++)
            {
                // 交叉
                var newChromosomes = _currentPopulation.CrossOver();

                // 新しい遺伝子の追加
                foreach (var newChromosome in newChromosomes)
                {
                    _nextPopulation.AddGene(newChromosome, _solverInfo.CalcFunc);
                }
            }

            // 突然変異
            for (int j = 0; j < _nextPopulation.PopulationSize; j++)
            {
                _nextPopulation.Mutation(j);
            }

            // 適用度の計算
            _nextPopulation.CalcAllFittness();

            // 淘汰
            _nextPopulation.DecreasePopulation();

            _currentPopulation = _nextPopulation;
        }



        #endregion

        /// <summary>
        /// 現在の世代の適用度をすべて取得します
        /// </summary>
        public List<double> GetCurrentFittnessAll()
        {
            return _currentPopulation.GetFittnessAll();
        }

        /// <summary>
        /// 現在の世代の適用度の平均を返します
        /// </summary>
        public double GetCurrentAverageFittness()
            => _currentPopulation.GetAverageFittness();

        /// <summary>
        /// 現在の世代の適用度の最大値を返します
        /// </summary>
        public double GetCurrentTopFittness()
            => _currentPopulation.GetTopFittness();

        /// <summary>
        /// 現在の世代の中で最大の個体を取得します
        /// </summary>
        public IGene<int> GetCurrentTopGene()
        {
            return _currentPopulation.GetTopGene();
        }

        /// <summary>
        /// 現在の世代数を返します
        /// </summary>
        public int GetCurrentGeneration()
        {
            return _currentPopulation.GenerationNum;
        }

        /// <summary>
        /// ソルバーの情報を成型して返します
        /// </summary>
        public string GetSolverInfo()
        {
            return $"ps={_solverInfo.InitialPopulationSize},st={_solverInfo.SelectionType},crt={_solverInfo.CrossOverType},mt={_solverInfo.MutationType},mr={_solverInfo.MutationRate}";
        }
    }
}
