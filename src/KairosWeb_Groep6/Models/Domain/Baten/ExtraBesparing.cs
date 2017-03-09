namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class ExtraBesparing : KostOfBaat
    {
        //Beschrijving wordt niet gebruikt      
        #region Constructors
        public ExtraBesparing()
        {
            Type = Type.Baat;
            Soort = Soort.ExtraBesparing;
        }
        #endregion
    }
}
