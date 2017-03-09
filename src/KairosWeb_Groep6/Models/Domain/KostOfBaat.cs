namespace KairosWeb_Groep6.Models.Domain
{
    public abstract class KostOfBaat
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public virtual double Bedrag { get; set; }

        protected KostOfBaat()
        {
            
        }

        //ICollection<KolomWaarde> Kolommen { get; set; }
        //ICollection<Rij> Waarden { get; set; }
        //double GetBedrag(int rijNr);
        //double BerekenTotaal();
        //void StelKolommenIn();
    }
}
