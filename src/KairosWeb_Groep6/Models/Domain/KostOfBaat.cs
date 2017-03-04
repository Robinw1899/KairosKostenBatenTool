using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface KostOfBaat
    {
        int Id { get; set; }
        Type Type { get; set; }
        Soort Soort { get; set; }
        //ICollection<KolomWaarde> Kolommen { get; set; }
        //ICollection<Rij> Waarden { get; set; }
        //double GetBedrag(int rijNr);
        //double BerekenTotaal();
        //void StelKolommenIn();
    }
}
