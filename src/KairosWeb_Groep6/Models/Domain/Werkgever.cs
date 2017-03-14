namespace KairosWeb_Groep6.Models.Domain
{
    public class Werkgever
    {
        public int WerkgeverId { get; set; }

        public string Naam { get; set; }

        public string Straat { get; set; }

        public int Nummer { get; set; }

        public int Postcode { get; set; }

        public string Gemeente { get; set; }

        public static int AantalWerkuren { get; set; }

        public static int PatronaleBijdrage { get; set; } = 35;

        public Werkgever()

        {
            
        }

        public Werkgever(string naam, string straat, int nummer, int postcode, string gemeente, int aantalWerkuren)
            : this(naam, straat, nummer, postcode, gemeente, aantalWerkuren, 35)
        {
            
        }

        public Werkgever(string naam, string straat, int nummer, int postcode, string gemeente, int aantalWerkuren, int patronaleBijdrage)
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
