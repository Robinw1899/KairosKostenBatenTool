using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class GebruikerViewModel
    {
        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        public Organisatie Organisatie { get; set; }

        public ICollection<Analyse> Analyses { get; private set; }

        public GebruikerViewModel(Gebruiker gebruiker)
        {
            Naam = gebruiker.Naam;
            Voornaam = gebruiker.Voornaam;
            Emailadres = gebruiker.Emailadres;
            Organisatie = gebruiker.Organisatie;
            Analyses = gebruiker.Analyses;
        }
    }
}
