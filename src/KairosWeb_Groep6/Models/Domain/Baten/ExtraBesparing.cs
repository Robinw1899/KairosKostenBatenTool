namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class ExtraBesparing : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; }
        #endregion

        #region Constructors
        public ExtraBesparing()
        {
            Type = Type.Baat;
            Soort = Soort.ExtraBesparing;
        }
        #endregion
    }
}
