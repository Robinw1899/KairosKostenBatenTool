namespace KairosWeb_Groep6.Models.Domain
{
    public class Organisatie
    {
        public int OrganisatieId { get; set; }

        public string Naam { get; set; }

        public string Straat { get; set; }

        public int Nummer { get; set; }

        public string Bus { get; set; } = "";

        public int Postcode { get; set; }

        public string Gemeente { get; set; }

        public Organisatie()
        {
            
        }

        public Organisatie(string naam, string straat, int nummer, int postcode, string gemeente)
        {
            Naam = naam;
            Straat = straat;
            Nummer = nummer;
            Postcode = postcode;
            Gemeente = gemeente;
        }
    }
}
