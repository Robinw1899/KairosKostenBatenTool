namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class LogistiekeBesparing : KostOfBaat
    {
        #region Properties
        // beschrijving en bedrag worden niet gebruikt
        public int TransportKosten { get; set; }

        public int LogistiekHandlingsKosten { get; set; }
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
