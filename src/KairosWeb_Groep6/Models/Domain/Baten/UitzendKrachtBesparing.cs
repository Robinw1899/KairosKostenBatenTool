namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class UitzendKrachtBesparing : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; } // = kolom jaarbedrag
        #endregion

        #region Constructors
        public UitzendKrachtBesparing()
        {
            Type = Type.Baat;
            Soort = Soort.UitzendkrachtBesparing;
        }
        #endregion

    }
}
