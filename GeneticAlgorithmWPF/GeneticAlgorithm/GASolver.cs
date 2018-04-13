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
        private Population _currentPopulation;
        private Population _nextPopulation;

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
            // 適用度の計算
            _currentPopulation.CalcAllFittness();

            for (int i = 0; i < _solverInfo.InitialPopulationSize; i++)
            {
                // 選択
                var selectedIndex = _currentPopulation.Selection();
                // 交叉
            }


        }

        #endregion
    }
}
