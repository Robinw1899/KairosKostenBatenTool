namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class BegeleidingsKost : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; }
        #endregion

        #region Constructors
        public BegeleidingsKost()
        {
            Type = Type.Kost;
            Soort = Soort.BegeleidingsKost;
        }
        #endregion
    }
}
