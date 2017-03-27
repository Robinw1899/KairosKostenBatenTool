﻿using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using System.Linq;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class IndexViewModel
    {
        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        public Organisatie Organisatie { get; set; }

        public IEnumerable<AnalyseViewModel> Analyses { get; private set; }

        public IndexViewModel()
        {
            Analyses = new List<AnalyseViewModel>();
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
        }
    }
}