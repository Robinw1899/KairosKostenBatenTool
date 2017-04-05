using System.Collections.Generic;
using System.Linq;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class AnalyseExtensions
    {
        public static IEnumerable<Analyse> InArchief(this ICollection<Analyse> analyses)
        {
            return analyses
                .Where(a => a.InArchief)
                .ToList();
        }

        public static IEnumerable<Analyse> NietInArchief(this ICollection<Analyse> analyses)
        {
            return analyses
                .Where(a => a.InArchief == false)
                .ToList();
        }
    }
}
