namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class ExtraOmzet : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; } // wordt niet gebruikt
        public double JaarbedragOmzetverlies { get; set; }
        public double Besparing { get; set; } // = % --> bv. 10%

        public double Bedrag
        {
            get { return JaarbedragOmzetverlies * (Besparing / 100); }
            set { }
        }

        #endregion

        #region Constructors
        public ExtraOmzet()
        {
            Type = Type.Baat;
            Soort = Soort.ExtraOmzet;
        }
        #endregion
    }
}
