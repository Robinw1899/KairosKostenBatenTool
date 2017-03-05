using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public class ZelfdeNiveauBaat : KostOfBaat
    {
        public ICollection<KolomWaarde> Kolommen { get; set; }     
        public Type type { get; set; }
        public ICollection<Rij> waarden { get; set; }
        
        public ZelfdeNiveauBaat()
        {
            Kolommen = new List<KolomWaarde>();
            waarden = new List<Rij>();
            type = Type.Baat;
        }

        public double BerekenTotaal()
        {
            throw new NotImplementedException();
        }

        public double GetBedrag(int rijNr)
        {
            throw new NotImplementedException();
        }
        public double berekenTotaleLoonkostPerJaar(int rijNr)
        {
            throw new NotImplementedException();
        }
        public double berekenTotaalLoonkostPerJaarAlle()
        {
            throw new NotImplementedException();
        }

    }
}
