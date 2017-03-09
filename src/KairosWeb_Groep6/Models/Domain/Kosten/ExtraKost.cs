namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class ExtraKost : KostOfBaat
    {      
        #region Constructors
        public ExtraKost()
        {
            Type = Type.Kost;
            Soort = Soort.ExtraKost;
        }
        #endregion
    }
}
