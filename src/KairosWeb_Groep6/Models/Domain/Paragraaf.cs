namespace KairosWeb_Groep6.Models.Domain
{
    public class Paragraaf
    {
        #region Properties
        public int ParagraafId { get; set; }

        // Dit wordt erbij gezet omdat de volgorde van paragrafen in een db verloren gaat:
        public int Volgnummer { get; set; }

        public string Tekst { get; set; }
        #endregion
    }
}
