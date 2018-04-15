using GeneitcAlgorithmWPF.Utility;
using GeneticAlgorithmWPF.Caching;
using GeneticAlgorithmWPF.GeneticAlgorithm;
using GeneticAlgorithmWPF.GeneticAlgorithm.Utility;
using GeneticAlgorithmWPF.TravellingSalesmanProblem.Model;
using GeneticAlgorithmWPF.TravellingSalesmanProblem.Utility;
using GeneticAlgorithmWPF.Utility;
using GeneticAlgorithmWPF.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GeneticAlgorithmWPF
{
    public enum ExecutePhase
    {
        DataReading,
        Setting,
        ExecuteReady,
        RunningOneStep,
        RunningAll,
        ExecuteDone,
    }

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly SolidColorBrush CITY_BRUSH = new SolidColorBrush
        {
            Color = Color.FromRgb(50, 205, 50),
        };
        private static readonly SolidColorBrush CITY_STROKE_BRUSH = new SolidColorBrush
        {
            Color = Color.FromRgb(128, 128, 128),
        };
        private static readonly SolidColorBrush PATH_BRUSH = new SolidColorBrush
        {
            Color = Color.FromRgb(50, 205, 50),
        };
        private static readonly Color CHART_PLOT_COLOR = Color.FromRgb(50, 205, 50);
        private static readonly double CITY_ELLIPSE_SIZE = 20;
        private static readonly double CITY_ELLIPSE_STROKE_SIZE = 5;
        private static readonly double CITY_PATH_STROKE_SIZE = 2;
        private static readonly double CITY_DRAW_EXPAND_RATIO = 50;
        private static readonly string LOG_FILE_DIRECTORY = @"C:\Users\A14753\Documents\Visual Studio 2017\Projects\GeneticAlgorithmWPF\log\";

        private List<City> _cityList = new List<City>();
        private List<Ellipse> _cityEllipseList = new List<Ellipse>();

        private List<double> _topFittnessList = new List<double>();
        private List<double> _averageFittnessList = new List<double>();

        private GASolver _gaSolver;

        private int _maxGeneration;
        private int _logDisplayNum;

        private ExecutePhase _executePhase;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            UpdatePhase(ExecutePhase.DataReading);
        }

        /// <summary>
        /// ボタンの有効状態を更新します
        /// </summary>
        private void UpdateButtonState()
        {
            switch (_executePhase)
            {
                case ExecutePhase.DataReading:
                    ExecuteAllButton.IsEnabled = false;
                    ExecuteOneStepButton.IsEnabled = false;
                    ConfigButton.IsEnabled = false;
                    InputFileNameButton.IsEnabled = true;
                    WriteLogButton.IsEnabled = false;
                    break;
                case ExecutePhase.Setting:
                    ExecuteAllButton.IsEnabled = false;
                    ExecuteOneStepButton.IsEnabled = false;
                    ConfigButton.IsEnabled = true;
                    InputFileNameButton.IsEnabled = true;
                    WriteLogButton.IsEnabled = false;
                    break;
                case ExecutePhase.ExecuteReady:
                    ExecuteAllButton.IsEnabled = true;
                    ExecuteOneStepButton.IsEnabled = true;
                    ConfigButton.IsEnabled = true;
                    InputFileNameButton.IsEnabled = true;
                    WriteLogButton.IsEnabled = false;
                    break;
                case ExecutePhase.RunningOneStep:
                    ExecuteAllButton.IsEnabled = false;
                    ExecuteOneStepButton.IsEnabled = true;
                    ConfigButton.IsEnabled = false;
                    InputFileNameButton.IsEnabled = false;
                    WriteLogButton.IsEnabled = false;
                    break;
                case ExecutePhase.RunningAll:
                    ExecuteAllButton.IsEnabled = false;
                    ExecuteOneStepButton.IsEnabled = false;
                    ConfigButton.IsEnabled = false;
                    InputFileNameButton.IsEnabled = false;
                    WriteLogButton.IsEnabled = false;
                    break;
                case ExecutePhase.ExecuteDone:
                    ExecuteAllButton.IsEnabled = true;
                    ExecuteOneStepButton.IsEnabled = true;
                    ConfigButton.IsEnabled = true;
                    InputFileNameButton.IsEnabled = true;
                    WriteLogButton.IsEnabled = true;
                    break;
            }
        }

        /// <summary>
        /// 実行フェーズを更新します
        /// </summary>
        private void UpdatePhase(ExecutePhase nextPhase)
        {
            _executePhase = nextPhase;

            UpdateButtonState();
        }

        /// <summary>
        /// モデルの読み込み
        /// </summary>
        private void InputFileNameButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePhase(ExecutePhase.DataReading);

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = "Resources",
                FileName = "",
                DefaultExt = "*.*",
                Filter = "CSVファイル|*.csv",
            };

            var ret = ofd.ShowDialog();
            if (ret.HasValue && ret.Value)
            {
                var filePath = ofd.FileName;
                InputFileNameLabel.Content = ofd.SafeFileName;

                var datas = CsvUtility.ReadCsv(filePath, true);

                _cityList.Clear();
                _cityList = ModelUtility.GetAllCitiesModel(datas).ToList();
            }

            DrawCities();

            UpdatePhase(ExecutePhase.Setting);
        }

        /// <summary>
        /// 設定ウィンドウを開きます
        /// </summary>
        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var settingWindow = new SettingWindow();
            var ret = settingWindow.ShowDialog();
            if (ret.HasValue && ret.Value)
            {
                return;
            }

            var chromosomesType = (ChromosomesType)CachingConfig.SettingCaching.ChromosomesTypeIndex;
            var selectionType = (SelectionType)CachingConfig.SettingCaching.SelectionTypeIndex;
            var crossOverType = (CrossOverType)CachingConfig.SettingCaching.CrossOverTypeIndex;
            var mutationType = (MutationType)CachingConfig.SettingCaching.MutationTypeIndex;
            var mutationRate = CachingConfig.SettingCaching.MutationRate;
            var populationSize = CachingConfig.SettingCaching.PopulationSize;

            // ソルバーの生成
            _gaSolver = new GASolver(new GASolver.GASolverInfo(
                chromosomesType,
                selectionType,
                crossOverType,
                mutationType,
                mutationRate,
                populationSize,
                _cityList.Count,
                CalculatePathCost));
            _gaSolver.Initialize();

            _maxGeneration = CachingConfig.SettingCaching.MaxGeneration;
            _logDisplayNum = CachingConfig.SettingCaching.LogDisplayNum;

            UpdatePhase(ExecutePhase.ExecuteReady);
        }

        /// <summary>
        /// パスのコストを計算します
        /// </summary>
        private double CalculatePathCost(List<int> path)
        {
            var cityNum = _cityList.Count;
            for (int i = 0; i < cityNum; i++)
            {
                _cityList[path[i]].Order = i;
            }

            return CityUtility.GetAllPathCost(_cityList.OrderBy(x => x.Order).ToArray());
        }

        /// <summary>
        /// 都市を描画します
        /// </summary>
        private void DrawCities()
        {
            PathCanvas.Children.Clear();
            _cityEllipseList.Clear();

            foreach (var city in _cityList)
            {
                var ellipse = new Ellipse
                {
                    Fill = CITY_BRUSH,
                    Width = CITY_ELLIPSE_SIZE,
                    Height = CITY_ELLIPSE_SIZE,
                    Margin = new Thickness
                    {
                        Left = CITY_DRAW_EXPAND_RATIO * city.X - CITY_ELLIPSE_SIZE / 2,
                        Top = CITY_DRAW_EXPAND_RATIO * city.Y - CITY_ELLIPSE_SIZE / 2,
                    },
                    Stroke = CITY_STROKE_BRUSH,
                    StrokeThickness = CITY_ELLIPSE_STROKE_SIZE,
                };

                PathCanvas.Children.Add(ellipse);
                _cityEllipseList.Add(ellipse);
            }
        }

        /// <summary>
        /// パスを描画します
        /// </summary>
        private void DrawPath(int[] path)
        {
            var canvasChildCount = PathCanvas.Children.Count;
            if (canvasChildCount > _cityList.Count)
            {
                PathCanvas.Children.RemoveRange(_cityList.Count, canvasChildCount);
            }

            var pathLength = path.Length;
            for (int i = 0; i < pathLength; i++)
            {
                var cityFrom = _cityList[path[i]];
                var cityTo = _cityList[i < pathLength - 1 ? path[i + 1] : path[0]];
                var line = new Line
                {
                    X1 = CITY_DRAW_EXPAND_RATIO * cityFrom.X,
                    Y1 = CITY_DRAW_EXPAND_RATIO * cityFrom.Y,
                    X2 = CITY_DRAW_EXPAND_RATIO * cityTo.X,
                    Y2 = CITY_DRAW_EXPAND_RATIO * cityTo.Y,
                    Stroke = CITY_STROKE_BRUSH,
                    StrokeThickness = CITY_PATH_STROKE_SIZE,
                };

                PathCanvas.Children.Add(line);
            }
        }

        /// <summary>
        /// １ステップ実行ボタン押下時の処理
        /// </summary>
        private void ExecuteOneStepButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePhase(ExecutePhase.RunningOneStep);

            ExecuteOneStep();

            UpdatePhase(ExecutePhase.ExecuteReady);
        }

        /// <summary>
        /// 全て実行ボタン押下時の処理
        /// </summary>
        private void ExecuteAllButton_Click(object sender, RoutedEventArgs e)
        {
//            for (int i = 0; i < _maxGeneration; i++)
//            {
//                ExecuteOneStep();
//            }         

            ExecuteWithAnimation();
        }

        /// <summary>
        /// アニメーション付きで実行します
        /// </summary>
        private void ExecuteWithAnimation()
        {
            var itteration = 0;

            // n秒後に処理を実行
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.1) };
            timer.Start();
            timer.Tick += (s, args) =>
            {
                ExecuteOneStep();
                itteration++;

                if (itteration > _maxGeneration)
                {
                    UpdatePhase(ExecutePhase.ExecuteDone);

                    // タイマーの停止
                    timer.Stop();
                }
            };
        }

        /// <summary>
        /// １ステップ実行します
        /// </summary>
        private void ExecuteOneStep()
        {
            // 1世代分進化させる
            _gaSolver.SolveOneStep();

            // 適用度が最大の個体を描画
            var topGene = _gaSolver.GetCurrentTopGene();
            DrawPath(topGene.Chromosomes.ToArray());

            // ログ表示
            DisplayLog(_logDisplayNum);
            DisplaySolverInfo();

            //            DrawFittnessGraph(new[] { 3.4, 1, 2, 5, 7.8, 2, 3 }, true);
        }

        /// <summary>
        /// 適用度グラフをすべて描画します
        /// </summary>
        private void DrawFittnessGraph(double[] y, bool clearFlag = false)
        {
            var series = FittnessChart?.Series?[0];
            if (series == null)
            {
                return;
            }

            if (clearFlag)
            {
                series.Points.Clear();
            }

            series.Points.Add(y);
        }

        /// <summary>
        /// リストボックスにログを出力します
        /// </summary>
        private void DisplayLog(int displayNum)
        {
            LogListBox.Items.Clear();

            var fittnessListSorted = _gaSolver.GetCurrentFittnessAll().Select((v, i) => new { Index = i, Value = v }).OrderBy(x => x.Value).ToArray();
            var displayFittness = fittnessListSorted.Take(displayNum);
            var topFittness = fittnessListSorted.First().Value;

            LogListBox.Items.Add($"TopFittness: {topFittness}");
            
            foreach (var fittness in displayFittness)
            {
                LogListBox.Items.Add($"Gene[{fittness.Index}]: {fittness.Value}");
            }

            _topFittnessList.Add(topFittness);
            _averageFittnessList.Add(_gaSolver.GetCurrentAverageFittness());
        }

        /// <summary>
        /// GASolverの情報を表示します
        /// </summary>
        private void DisplaySolverInfo()
        {
            SolverInfoLabel.Content =
                $"Generation: {_gaSolver.GetCurrentGeneration()}  TopFittness: {_gaSolver.GetCurrentTopFittness():F3}";
        }

        /// <summary>
        /// ログ出力ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WriteLogButton_Click(object sender, RoutedEventArgs e)
        {
            var date = DateTime.Today;
            var dateDirectoryName = date.Year.ToString("0000") + date.Month.ToString("00") + date.Day.ToString("00") + "/";

            // logフォルダ作成
            var directoryInfo = FileUtility.SafeCreateDirectory(LOG_FILE_DIRECTORY + dateDirectoryName);

            CsvUtility.WriteCsv(_topFittnessList, directoryInfo.FullName + "log.csv");
        }
    }
}
