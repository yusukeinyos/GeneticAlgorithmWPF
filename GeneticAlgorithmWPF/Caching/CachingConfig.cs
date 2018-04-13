using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace GeneticAlgorithmWPF.Caching
{
    public static class CachingConfig
    {
        public static SettingCaching SettingCaching => _settingCaching ?? (_settingCaching = SettingCaching.Instance);
        private static SettingCaching _settingCaching;

        public static void Save()
        {
            _settingCaching?.Save();
        }
    }
}
