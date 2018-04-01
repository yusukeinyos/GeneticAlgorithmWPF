using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmWPF.Utility
{
    public static class LinqExtensions
    {
        /// <summary>
        /// シーケンスをシャッフルします
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence)
            => sequence.OrderBy(x => Guid.NewGuid());
    }
}
