using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.ExtraKostViewModels
{
    public class ExtraKostViewModel
    {
        #region Properties
        public int Id { get; set; }

        public Type Type { get; set; }

        public Soort Soort { get; set; }

        public string Beschrijving { get; set; }

        public double Bedrag { get; set; }
        #endregion

        #region Constructors

        public ExtraKostViewModel()
        {
            
        }

        public ExtraKostViewModel(ExtraKost kost)
        {
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Beschrijving = kost.Beschrijving;
            Bedrag = kost.Bedrag;
        }
        #endregion

    }
}
