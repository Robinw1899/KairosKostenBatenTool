namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class GereedschapsKost : KostOfBaat
    {       
        #region Constructors
        public GereedschapsKost()
        {
            Type = Type.Kost;
            Soort = Soort.GereedschapsKost;
        }
        #endregion
    }
}
