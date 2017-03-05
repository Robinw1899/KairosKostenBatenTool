﻿namespace KairosWeb_Groep6.Models.Domain
{
    public class Werkgever
    {
        public string Naam { get; set; }
        public string Straat { get; set; }
        public int Nummer { get; set; }
        public int Postcode { get; set; }
        public string Gemeente { get; set; }
        public int AantalWerkuren { get; set; }
        public double PatronaleBijdrage { get; set; }

        public Werkgever(string naam, string straat, int nummer, int postcode, string gemeente, int aantalWerkuren)
            : this(naam, straat, nummer, postcode, gemeente, aantalWerkuren, 35.0)
        {
            
        }

        public Werkgever(string naam, string straat, int nummer, int postcode, string gemeente, int aantalWerkuren, double patronaleBijdrage)
        {
            Naam = naam;
            Straat = straat;
            Nummer = nummer;
            Postcode = postcode;
            Gemeente = gemeente;
            AantalWerkuren = aantalWerkuren;
            PatronaleBijdrage = patronaleBijdrage;
        }
    }
}
