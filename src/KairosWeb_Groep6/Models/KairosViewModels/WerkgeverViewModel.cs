using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class WerkgeverViewModel
    {
        public string Naam { get; set; }
        public string Straat { get; set; }
        public string Nummer { get; set; }
        public int Postcode { get; set; }
        public string Gemeente { get; set; }
        public static int AantalWerkuren { get; set; }
        public static double PatronaleBijdrage { get; set; } = 0.35D;

        public WerkgeverViewModel()
        {
            
        }
        public WerkgeverViewModel(Werkgever werkgever)
        {
            Naam = werkgever.Naam;
            Straat = werkgever.Straat;
            Nummer = werkgever.Nummer;
            Postcode = werkgever.Postcode;
            Gemeente = werkgever.Gemeente;
            AantalWerkuren = Werkgever.AantalWerkuren;
            PatronaleBijdrage = Werkgever.PatronaleBijdrage;
        }

    }
}


