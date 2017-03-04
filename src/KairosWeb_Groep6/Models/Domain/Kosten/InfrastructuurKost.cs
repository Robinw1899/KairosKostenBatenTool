namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class InfrastructuurKost : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Soort Soort { get; set; }
        public Type Type { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; }
        #endregion

        #region Constructors
        public InfrastructuurKost()
        {
            Type = Type.KOST;
            Soort = Soort.InfrastructuurKost;
        }
        #endregion
    }
}
