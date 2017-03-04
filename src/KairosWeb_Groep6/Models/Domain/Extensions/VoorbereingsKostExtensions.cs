using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class VoorbereingsKostExtensions
    {
        public static VoorbereidingsKost GetBy(this List<VoorbereidingsKost> voorbereidingsKosten, int id)
        {
            return voorbereidingsKosten
                .SingleOrDefault(v => v.Id == id);
        }

        public static double GeefBedrag(this List<VoorbereidingsKost> voorbereidingsKosten, int id)
        {
            return voorbereidingsKosten
                .SingleOrDefault(v => v.Id == id)
                .Bedrag;
        }

        public static double GeefTotaal(this List<VoorbereidingsKost> voorbereidingsKosten)
        {
            return voorbereidingsKosten
                .Sum(v => v.Bedrag);
        }
    }
}
