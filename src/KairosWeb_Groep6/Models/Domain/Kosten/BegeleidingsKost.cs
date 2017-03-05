using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public class BegeleidingsKost : KostOfBaat
    {
        public ICollection<KolomWaarde> kolommen { get; set; }
        public ICollection<Rij> waarden { get; set; }

        public Type type { get; set; }

        public BegeleidingsKost()
        {
            kolommen = new List<KolomWaarde>();
            waarden = new List<Rij>();
            type = Type.KOST;
        }
        public double berekenTotaal()
        {
            throw new NotImplementedException();
        }

        public double getBedrag(int rijNr)
        {
            throw new NotImplementedException();
        }
    }
}
