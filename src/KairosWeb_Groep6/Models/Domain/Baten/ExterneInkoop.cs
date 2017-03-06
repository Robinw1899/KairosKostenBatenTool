namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class ExterneInkoop : KostOfBaat
    {
        // Transportkosten en logistieke handlingskosten horen in deze klasse
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; } // = kolom "jaarbedrag"
        #endregion

        #region Constructors
        public ExterneInkoop()
        {
            Type = Type.Baat;
            Soort = Soort.ExterneInkoop;
        }
        #endregion
    }
}
