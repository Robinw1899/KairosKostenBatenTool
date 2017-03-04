namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class EnclaveKost : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Jaarbedrag { get; set; }
        #endregion

        #region Constructors
        public EnclaveKost()
        {
            Type = Type.KOST;
            Soort = Soort.EnclaveKost;
        }
        #endregion
    }
}
