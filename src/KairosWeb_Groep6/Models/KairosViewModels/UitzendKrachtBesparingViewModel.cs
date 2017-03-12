﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;


namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class UitzendKrachtBesparingViewModel : IEnumerable
    {
        public int Id { get; set; }
        public Type Type { get; set; }
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