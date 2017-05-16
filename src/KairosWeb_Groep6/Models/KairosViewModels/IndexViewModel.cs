using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class IndexViewModel
    {
        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        public Organisatie Organisatie { get; set; }

        public IEnumerable<AnalyseViewModel> Analyses { get; set; }

        public int BeginIndex { get; set; }

        public int EindIndex { get; set; }

        public bool ShowVolgende { get; set; }
        public bool ShowVorige { get; set; }

        public IEnumerable<SelectListItem> listItems { get; set; }

        public int DatumId { get; set; }
        public IndexViewModel()
        {
            Analyses = new List<AnalyseViewModel>();
            listItems = new List<SelectListItem>();
        }

        public IndexViewModel(Jobcoach gebruiker)
        {
            Naam = gebruiker.Naam;
            Voornaam = gebruiker.Voornaam;
            Emailadres = gebruiker.Emailadres;
            Organisatie = gebruiker.Organisatie;
            Analyses = gebruiker.Analyses
                                    .Select(a => new AnalyseViewModel(a))
                                    .ToList();
            listItems = new List<SelectListItem>();
        }



    }
}
