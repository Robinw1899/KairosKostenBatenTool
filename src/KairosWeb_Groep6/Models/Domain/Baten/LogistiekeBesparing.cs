namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class LogistiekeBesparing : KostOfBaat
    {
        #region Properties
        // beschrijving en bedrag worden niet gebruikt
        public decimal TransportKosten { get; set; } // B.10.1

        public decimal LogistiekHandlingsKosten { get; set; } // B.10.2

        public override decimal Bedrag
        {
            get { return TransportKosten + LogistiekHandlingsKosten; }
            set { }
        }
        #endregion

        #region Constructors

        public LogistiekeBesparing()
        {
            Type = Type.Baat;
            Soort = Soort.LogistiekeBesparing;
        }
        #endregion
    }
}
