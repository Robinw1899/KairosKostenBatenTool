namespace KairosWeb_Groep6.Models.Domain
{
    public class KostOfBaat
    {
        #region Properties        
        public int Id { get; set; }

        
        public Type Type { get; set; }

        
        public Soort Soort { get; set; }

        
        public string Beschrijving { get; set; }

        
        public virtual decimal Bedrag { get; set; }
        #endregion

        #region Constructors
        protected KostOfBaat()
        {
            
        }
        #endregion
    }
}
