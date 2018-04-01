using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using GeneitcAlgorithmWPF.Utility;
using GeneticAlgorithmWPF.TravellingSalesmanProblem.Model;
using GeneticAlgorithmWPF.TravellingSalesmanProblem.Utility;
using GeneticAlgorithmWPF.Utility;
using GeneticAlgorithmWPF.View;

namespace GeneticAlgorithmWPF
{
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
        private static readonly double CITY_ELLIPSE_SIZE = 20;
        private static readonly double CITY_ELLIPSE_STROKE_SIZE = 5;
        private static readonly double CITY_PATH_STROKE_SIZE = 2;
        private static readonly double CITY_DRAW_EXPAND_RATIO = 50;

        private List<City> _cityList = new List<City>();
        private List<Ellipse> _cityEllipseList = new List<Ellipse>();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// モデルの読み込み
        /// </summary>
        private void InputFileNameButton_Click(object sender, RoutedEventArgs e)
        {
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
        }

        /// <summary>
        /// 都市を描画します
        /// </summary>
        private void DrawCities()
        {
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
                var cityFrom = _cityList[path[i] - 1];
                var cityTo = _cityList[i < pathLength - 1 ? path[i + 1] - 1 : path[0] - 1];
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
        /// 全て実行します
        /// </summary>
        private void ExecuteAllButton_Click(object sender, RoutedEventArgs e)
        {
            var path = Enumerable.Range(1, 7).Shuffle().ToArray();
            DrawPath(path);
        }
    }
}
