namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class EnclaveKost : KostOfBaat
    {              
        // Bedrag = kolom "jaarbedrag"       
        #region Constructors
        public EnclaveKost()
        {
            Type = Type.Kost;
            Soort = Soort.EnclaveKost;
        }
        #endregion
    }
}
