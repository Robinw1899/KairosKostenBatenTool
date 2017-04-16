using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public class BegeleidingsKostExtensions
    {
        public static decimal GeefTotaal(List<BegeleidingsKost> kosten, decimal patronaleBijdrage)
        {
            return kosten
                .Sum(k => k.GeefJaarbedrag(patronaleBijdrage));
        }
    }
}
