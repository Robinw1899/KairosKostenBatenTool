using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.BegeleidingsKostViewModels
{
    public class BegeleidingsKostViewModel
    {
        #region Properties
        public int Id { get; set; }

        public Type Type { get; set; }

        public Soort Soort { get; set; }

        [Required(ErrorMessage = "Gelieve een aantal uren op te geven")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het aantal uur op te geven.")]
        public double Uren { get; set; }

        [Required(ErrorMessage = "Gelieve een bruto maandloon op te geven")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bruto maandloon op te geven.")]
        public double BrutoMaandloonBegeleider { get; set; }

        public double Bedrag { get; set; } // jaarbedrag van de huidige BegeleidingsKost
        #endregion

        #region Constructors
        public BegeleidingsKostViewModel()
        {

        }
        public BegeleidingsKostViewModel(BegeleidingsKost kost)
        {
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Uren = kost.Uren;
            BrutoMaandloonBegeleider = kost.BrutoMaandloonBegeleider;
        }
        #endregion
    }
}
