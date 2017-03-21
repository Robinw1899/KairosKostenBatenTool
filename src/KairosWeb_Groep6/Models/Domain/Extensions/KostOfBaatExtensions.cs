using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class KostOfBaatExtensions
    {
        public static T GetBy<T>(List<T> lijst, int id) where T : KostOfBaat
        {
            return lijst
                .SingleOrDefault(e => e.Id == id);
        }

        public static double GeefTotaal<T>(List<T> lijst) where T : KostOfBaat
        {
            if (typeof(T) == typeof(Loonkost))
            {
                throw new InvalidOperationException("Deze methode is niet voor de klasse Loonkost");
            }

            if (typeof(T) == typeof(MedewerkerNiveauBaat))
            {
                throw new InvalidOperationException("Deze methode is niet voor de klasse MedewerkerNiveauBaat");
            }

            if (typeof(T) == typeof(BegeleidingsKost))
            {
                throw new InvalidOperationException("Deze methode is niet voor de klasse BegeleidingsKost");
            }

            return lijst
                .Sum(v => v.Bedrag);
        }
    }
}
