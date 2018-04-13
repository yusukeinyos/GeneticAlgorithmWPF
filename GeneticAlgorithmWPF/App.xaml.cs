using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GeneticAlgorithmWPF.Caching;

namespace GeneticAlgorithmWPF
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            CachingConfig.Save();

            base.OnExit(e);
        }
    }
}
