namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class InfrastructuurKost : KostOfBaat
    {      
        #region Constructors
        public InfrastructuurKost()
        {
            Type = Type.Kost;
            Soort = Soort.InfrastructuurKost;
        }
        #endregion
    }
}
