namespace KairosWeb_Groep6.Models.Domain
{
    /**
     * Dit komt overeen met kost 1.1 van de Excel.
     */
    public class Functie : Kost
    {
        public string Naam { get; set; }
        public double AantalUrenPerWeek { get; set; }
        public double BrutoMaandloonFulltime { get; set; }
        public double Ondersteuningspremie { get; set; }
        public Doelgroep? Doelgroep { get; set; }
    }
}
