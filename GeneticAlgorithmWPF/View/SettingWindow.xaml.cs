using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GeneticAlgorithmWPF.Caching;
using GeneticAlgorithmWPF.Control;
using GeneticAlgorithmWPF.GeneticAlgorithm;
using GeneticAlgorithmWPF.GeneticAlgorithm.Utility;

namespace GeneticAlgorithmWPF.View
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public SettingWindow()
        {
            InitializeComponent();

            PopulationSizeComboBox.ItemsSource = new []
            {
                new ListItem(0, "100", 100),
                new ListItem(1, "200", 200),
                new ListItem(2, "500", 500),
                new ListItem(3, "1000", 1000),
                new ListItem(4, "2000", 2000),
                new ListItem(5, "5000", 5000),
                new ListItem(6, "10000", 10000),
            };
            // PopulationSizeComboBox.SelectedIndex = CachingConfig.SettingCaching.PopulationSize;
            GeneTypeComboBox.ItemsSource = Enum.GetValues(typeof(ChromosomesType));
            GeneTypeComboBox.SelectedIndex = CachingConfig.SettingCaching.ChromosomesTypeIndex;
            SelectionTypeComboBox.ItemsSource = Enum.GetValues(typeof(SelectionType));
            SelectionTypeComboBox.SelectedIndex = CachingConfig.SettingCaching.SelectionTypeIndex;
            CrossOverTypeComboBox.ItemsSource = Enum.GetValues(typeof(CrossOverType));
            CrossOverTypeComboBox.SelectedIndex = CachingConfig.SettingCaching.CrossOverTypeIndex;
            MutationTypeComboBox.ItemsSource = Enum.GetValues(typeof(MutationType));
            MutationTypeComboBox.SelectedIndex = CachingConfig.SettingCaching.MutationTypeIndex;
            MutationRateInput.Text = CachingConfig.SettingCaching.MutationRate.ToString();
            MaxGenerationInput.Text = CachingConfig.SettingCaching.MaxGeneration.ToString();
        }

        /// <summary>
        /// OKボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingApplyButton_Click(object sender, RoutedEventArgs e)
        {
            CachingConfig.SettingCaching.PopulationSize = GetItemListValue<int>(PopulationSizeComboBox);
            CachingConfig.SettingCaching.ChromosomesTypeIndex = GetListIndex(GeneTypeComboBox);
            CachingConfig.SettingCaching.SelectionTypeIndex = GetListIndex(SelectionTypeComboBox);
            CachingConfig.SettingCaching.CrossOverTypeIndex = GetListIndex(CrossOverTypeComboBox);
            CachingConfig.SettingCaching.MutationTypeIndex = GetListIndex(MutationTypeComboBox);
            CachingConfig.SettingCaching.MutationRate = MutationRateInput.ParseFloat(1);
            CachingConfig.SettingCaching.MaxGeneration = MaxGenerationInput.ParseInt(100);

            Close();
        }

        private static T GetListValue<T>(Selector comboBox) =>
            (T)comboBox.SelectedValue;

        private static int GetListIndex(Selector comboBox) =>
            comboBox.SelectedIndex;

        private static T GetItemListValue<T>(Selector comboBox) =>
            (T)((ListItem)comboBox.SelectedValue).Value;

        /// <summary>
        /// 一覧のアイテム
        /// </summary>
        private class ListItem : IEquatable<ListItem>
        {
            private readonly string _disp;

            /// <summary> 一意ID </summary>
            public byte Key { get; }

            /// <summary> 値 </summary>
            public object Value { get; }

            public ListItem(byte key, string disp, object value = null)
            {
                Key = key;
                _disp = disp;
                Value = value;
            }

            public override string ToString() => _disp;

            public bool Equals(ListItem obj) => obj != null && Key == obj.Key;
        }
    }


}
