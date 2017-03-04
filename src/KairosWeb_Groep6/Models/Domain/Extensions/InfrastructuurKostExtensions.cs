using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class InfrastructuurKostExtensions
    {
        public static InfrastructuurKost GetBy(this List<InfrastructuurKost> infrastructuurkosten, int id)
        {
            return infrastructuurkosten
                .SingleOrDefault(i => i.Id == id);
        }

        public static double GeefBedrag(this List<InfrastructuurKost> infrastructuurkosten, int id)
        {
            return infrastructuurkosten
                .SingleOrDefault(i => i.Id == id)
                .Bedrag;
        }

        public static double GeefTotaal(this List<InfrastructuurKost> infrastructuurkosten)
        {
            return infrastructuurkosten
                .Sum(i => i.Bedrag);
        }
    }
}
