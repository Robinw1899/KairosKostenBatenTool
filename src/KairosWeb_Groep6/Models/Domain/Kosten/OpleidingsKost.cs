namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class OpleidingsKost : KostOfBaat
    {      
       // Beschrijving = kolom "type"            
        #region Constructors
        public OpleidingsKost()
        {
            Type = Type.Kost;
            Soort = Soort.OpleidingsKost;
        }
        #endregion
    }
}
