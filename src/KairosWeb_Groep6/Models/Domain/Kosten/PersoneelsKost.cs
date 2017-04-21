namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class PersoneelsKost : KostOfBaat
    {      
        #region Constructors
        public PersoneelsKost()
        {
            Type = Type.Kost;
            Soort = Soort.PersoneelsKost;
        }
        #endregion
    }
}
