namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class VoorbereidingsKost : KostOfBaat
    {           
         //Beschrijving = kolom "type"      
        #region Constructors
        public VoorbereidingsKost()
        {
            Type = Type.Kost;
            Soort = Soort.VoorbereidingsKost;
        }
        #endregion
    }
}
