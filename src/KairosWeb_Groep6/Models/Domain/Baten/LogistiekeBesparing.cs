namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class LogistiekeBesparing : KostOfBaat
    {
        #region Properties
        // beschrijving en bedrag worden niet gebruikt
        public double TransportKosten { get; set; } // B.10.1

        public double LogistiekHandlingsKosten { get; set; } // B.10.2
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
