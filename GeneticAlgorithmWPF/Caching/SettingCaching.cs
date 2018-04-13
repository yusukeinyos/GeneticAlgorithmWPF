using GeneticAlgorithmWPF.Utility;
using System.Configuration;
using GeneticAlgorithmWPF.GeneticAlgorithm;
using GeneticAlgorithmWPF.GeneticAlgorithm.Utility;

namespace GeneticAlgorithmWPF.Caching
{
    public abstract class CachingBase<TClass> : ApplicationSettingsBase
        where TClass : CachingBase<TClass>, new()
    {
        public static TClass Instance => _instance ?? (_instance = new TClass());
        private static TClass _instance;
    }

    public class SettingCaching :CachingBase<SettingCaching>
    {
        [UserScopedSetting]
        public int PopulationSize
        {
            get => this["PopulationSize"].IsNull<int>();
            set => this["PopulationSize"] = value;
        }

        [UserScopedSetting]
        public int ChromosomesTypeIndex
        {
            get => this["ChromosomesTypeIndex"].IsNull<int>();
            set => this["ChromosomesTypeIndex"] = value;
        }

        [UserScopedSetting]
        public int SelectionTypeIndex
        {
            get => this["SelectionTypeIndex"].IsNull<int>();
            set => this["SelectionTypeIndex"] = value;
        }

        [UserScopedSetting]
        public int CrossOverTypeIndex
        {
            get => this["CrossOverTypeIndex"].IsNull<int>();
            set => this["CrossOverTypeIndex"] = value;
        }

        [UserScopedSetting]
        public int MutationTypeIndex
        {
            get => this["MutationTypeIndex"].IsNull<int>();
            set => this["MutationTypeIndex"] = value;
        }

        [UserScopedSetting]
        public float MutationRate
        {
            get => this["MutationRate"].IsNull<float>();
            set => this["MutationRate"] = value;
        }

        [UserScopedSetting]
        public int MaxGeneration
        {
            get => this["MaxGeneration"].IsNull<int>();
            set => this["MaxGeneration"] = value;
        }
    }
}
