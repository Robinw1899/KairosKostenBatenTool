using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.ExtraKostViewModels
{
    public class ExtraKostenIndexViewModel
    {
        #region Properties
        public int Id { get; set; }

        public Type Type { get; set; }

        public Soort Soort { get; set; }

        public string Beschrijving { get; set; }

        public double Bedrag { get; set; }

        public IEnumerable<ExtraKostViewModel> ViewModels { get; set; }
        #endregion

        #region Constructors

        public ExtraKostenIndexViewModel()
        {

        }

        public ExtraKostenIndexViewModel(ExtraKost kost)
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
