using Newtonsoft.Json;

namespace KairosWeb_Groep6.Models.Domain
{
    public class KostOfBaat
    {
        
        public int Id { get; set; }

        
        public Type Type { get; set; }

        
        public Soort Soort { get; set; }

        
        public string Beschrijving { get; set; }

        
        public virtual double Bedrag { get; set; }

        protected KostOfBaat()
        {
            
        }

       
        protected KostOfBaat(bool forJsonOnly)
        {
            
        }
    }
}
