using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public class ExtraKost : KostOfBaat
    {
        public ICollection<KolomWaarde> Kolommen { get; set; }
        public ICollection<Rij> waarden { get; set; }
        public Type type { get; set; }

        public ExtraKost()
        {
            Kolommen = new List<KolomWaarde>();
            waarden = new List<Rij>();
            type = Type.KOST;
        }

        public double berekenJaarbedrag(int rijNr)
        {
            throw new NotImplementedException();    
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
