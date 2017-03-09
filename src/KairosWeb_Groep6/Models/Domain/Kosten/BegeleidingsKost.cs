namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class BegeleidingsKost : KostOfBaat
    {       
        #region Constructors
        public BegeleidingsKost()
        {
            Type = Type.Kost;
            Soort = Soort.BegeleidingsKost;
        }
        #endregion
    }
}
