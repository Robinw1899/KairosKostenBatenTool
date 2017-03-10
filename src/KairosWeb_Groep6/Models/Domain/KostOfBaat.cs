using Newtonsoft.Json;

namespace KairosWeb_Groep6.Models.Domain
{
    public class KostOfBaat
    {
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public Type Type { get; set; }

        [JsonProperty]
        public Soort Soort { get; set; }

        [JsonProperty]
        public string Beschrijving { get; set; }

        [JsonProperty]
        public virtual double Bedrag { get; set; }

        protected KostOfBaat()
        {
            
        }

        [JsonConstructor]
        protected KostOfBaat(bool forJsonOnly)
        {
            
        }
    }
}
