namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class UitzendKrachtBesparing : KostOfBaat
    {       
       //Bedrag  = kolom jaarbedrag     
        #region Constructors
        public UitzendKrachtBesparing()
        {
            Type = Type.Baat;
            Soort = Soort.UitzendkrachtBesparing;
        }
        #endregion

    }
}
