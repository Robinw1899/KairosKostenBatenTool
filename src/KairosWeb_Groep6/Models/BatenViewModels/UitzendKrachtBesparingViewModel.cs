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
    public class UitzendKrachtBesparingViewModel : IEnumerable
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Domain.Type Type { get; set; }
        [HiddenInput]
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; }

        public UitzendKrachtBesparingViewModel()
        {

        }

        public UitzendKrachtBesparingViewModel(UitzendKrachtBesparing uitzendKracht)
        {
            Id = uitzendKracht.Id;
            Type = uitzendKracht.Type;
            Soort = uitzendKracht.Soort;
            Beschrijving = uitzendKracht.Beschrijving;
            Bedrag = uitzendKracht.Bedrag;
        }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
