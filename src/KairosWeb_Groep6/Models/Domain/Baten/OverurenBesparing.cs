namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class OverurenBesparing : KostOfBaat
    {              
        // Beschrijving wordt niet gebruikt        
        #region Constructors
        public OverurenBesparing()
        {
            Type = Type.Baat;
            Soort = Soort.OverurenBesparing;
        }
        #endregion
    }
}
