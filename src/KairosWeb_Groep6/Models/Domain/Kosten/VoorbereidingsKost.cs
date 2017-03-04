namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class VoorbereidingsKost : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Soort Soort { get; set; }
        public Type Type { get; set; } // = kolom "type"
        public string Beschrijving { get; set; }
        public int Bedrag { get; set; }
        #endregion

        #region Constructors
        public VoorbereidingsKost()
        {
            Type = Type.KOST;
            Soort = Soort.VoorbereidingsKost;
        }
        #endregion
    }
}
