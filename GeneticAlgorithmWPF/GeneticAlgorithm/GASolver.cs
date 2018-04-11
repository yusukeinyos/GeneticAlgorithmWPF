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
            public readonly int InitialPopulationSize;
            public readonly int GeneLength;

            public GASolverInfo(ChromosomesType chromosomesType, SelectionType selectionType, int initialPopulationSize, int geneLength)
            {
                ChromosomesType = chromosomesType;
                SelectionType = selectionType;
                InitialPopulationSize = initialPopulationSize;
                GeneLength = geneLength;
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
            _currentPopulation = new Population
            {
                ChromosomesType = _solverInfo.ChromosomesType,
                SelectionType = _solverInfo.SelectionType,
                InitialPopulationSize = _solverInfo.InitialPopulationSize,
                GeneLength = _solverInfo.GeneLength,
            };

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
