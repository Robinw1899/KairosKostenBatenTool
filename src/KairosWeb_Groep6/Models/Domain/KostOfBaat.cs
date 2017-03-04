using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface KostOfBaat
    {
        double GetBedrag(int rijNr);
        double BerekenTotaal();
        Type Type { get; set; }
        ICollection<KolomWaarde> kolommen {get;set;}
        ICollection<Rij> waarden { get;set; }
    }
}
