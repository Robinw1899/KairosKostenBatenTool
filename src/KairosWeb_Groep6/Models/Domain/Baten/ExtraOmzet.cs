namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class ExtraOmzet : KostOfBaat
    {
        #region Properties 
        //Beschrijving wordt niet gebruikt      
        public decimal JaarbedragOmzetverlies { get; set; }
        public decimal Besparing { get; set; } // = % --> bv. 10%

        public override decimal Bedrag
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
