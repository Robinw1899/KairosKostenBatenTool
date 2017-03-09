namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class Subsidie : KostOfBaat
    {               
        //Beschrijving wordt niet gebruikt
        #region Constructors
        public Subsidie()
        {
            Type = Type.Baat;
            Soort = Soort.Subsidie;
        }
        #endregion
    }
}
