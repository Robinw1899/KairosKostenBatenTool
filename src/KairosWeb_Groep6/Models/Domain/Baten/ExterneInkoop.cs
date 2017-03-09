namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class ExterneInkoop : KostOfBaat
    {
        // Transportkosten en logistieke handlingskosten horen in deze klasse      
       //bedrag = kolom "jaarbedrag"        
        #region Constructors
        public ExterneInkoop()
        {
            Type = Type.Baat;
            Soort = Soort.ExterneInkoop;
        }
        #endregion
    }
}
