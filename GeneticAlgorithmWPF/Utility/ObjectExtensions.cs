using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmWPF.Utility
{
    public static class ObjectExtensions
    {
        public static T IsNull<T>(this object obj, T defaultValue = default(T))
            => obj == null ? defaultValue : (T) obj;
    }
}
