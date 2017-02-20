using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class JobcoachViewModel
    {
        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        public Organisatie Organisatie { get; set; }

        public ICollection<Analyse> Analyses { get; private set; }

        public JobcoachViewModel(Jobcoach jobcoach)
        {
            Naam = jobcoach.Naam;
            Voornaam = jobcoach.Voornaam;
            Emailadres = jobcoach.Emailadres;
            Organisatie = jobcoach.Organisatie;
            Analyses = jobcoach.Analyses;
        }
    }
}
