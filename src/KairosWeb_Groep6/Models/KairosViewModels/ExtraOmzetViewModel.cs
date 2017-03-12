﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;


namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class ExtraOmzetViewModel
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; } // wordt niet gebruikt
        public double JaarbedragOmzetverlies { get; set; }
        public double Besparing { get; set; } // = % --> kommagetal, bv. 10% = 0,1

        public double Bedrag
        {
            get { return JaarbedragOmzetverlies * Besparing; }
            set { }
        }

        public ExtraOmzetViewModel()
        {
            
        }

        public ExtraOmzetViewModel(ExtraOmzet omzet)
        {
            Id = omzet.Id;
            Type = omzet.Type;
            Soort = omzet.Soort;
            Beschrijving = omzet.Beschrijving;
            JaarbedragOmzetverlies = omzet.JaarbedragOmzetverlies;
            Besparing = omzet.Besparing;
            Bedrag = omzet.Bedrag;
        }
    }
}