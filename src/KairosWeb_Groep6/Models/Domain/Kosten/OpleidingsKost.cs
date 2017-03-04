using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public class OpleidingsKost : KostOfBaat
    {
        public ICollection<KolomWaarde> Kolommen { get; set; }
        public ICollection<Rij> waarden { get; set; }
        public Type type { get; set; }

        public OpleidingsKost()
        {
            Kolommen = new List<KolomWaarde>();
            waarden = new List<Rij>();
            type = Type.KOST;
        }

        public double BerekenTotaal()
        {
            throw new NotImplementedException();
        }

        public double GetBedrag(int rijNr)
        {
            throw new NotImplementedException();
        }
    }
}
