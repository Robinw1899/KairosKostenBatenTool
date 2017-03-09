using System.Collections;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Common;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Werkgever
    {
        public int WerkgeverId { get; set; }
        public string Naam { get; set; }
        public string Straat { get; set; }
        public string Nummer { get; set; }
        public int Postcode { get; set; }
        public string Gemeente { get; set; }
        public static int AantalWerkuren { get; set; }

        public static double PatronaleBijdrage { get; set; } = 0.35D;
        public IEnumerable<Analyse> Analyses { get; set; }

        public Werkgever()

        {
            
        }

        public Werkgever(string naam, string straat, int nummer, int postcode, string gemeente, int aantalWerkuren)
            : this(naam, straat, nummer, postcode, gemeente, aantalWerkuren, 0.35D)
        {
            
        }

        public Werkgever(string naam, string straat, string nummer, int postcode, string gemeente, int aantalWerkuren, double patronaleBijdrage)
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
