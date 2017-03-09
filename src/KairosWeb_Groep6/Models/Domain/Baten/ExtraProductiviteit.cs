namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class ExtraProductiviteit : KostOfBaat
    {       
        //Beschrijving wordt niet gebruikt
        #region Constructors
        public ExtraProductiviteit()
        {
            Type = Type.Baat;
            Soort = Soort.ExtraProductiviteit;
        }
        #endregion
    }
}
