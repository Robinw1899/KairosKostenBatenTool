using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.BatenViewModels
{
    public class MedewerkerNiveauBaatViewModel : IEnumerable
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Domain.Type Type { get; set; }
        [HiddenInput]
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Uren { get; set; }
        public double Bedrag { get; set; }
        public double BrutoMaandloonFulltime { get; set; }

        public MedewerkerNiveauBaatViewModel()
        {

        }
        public MedewerkerNiveauBaatViewModel(MedewerkerNiveauBaat medewerker)
        {
            Id = medewerker.Id;
            Type = medewerker.Type;
            Soort = medewerker.Soort;
            Beschrijving = medewerker.Beschrijving;
            Uren = medewerker.Uren;
            Bedrag = medewerker.Bedrag;
            BrutoMaandloonFulltime = medewerker.BrutoMaandloonFulltime;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
