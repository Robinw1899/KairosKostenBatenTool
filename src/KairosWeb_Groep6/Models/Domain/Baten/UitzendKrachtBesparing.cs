﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public class UitzendKrachtBesparing:KostOfBaat
    {
        public ICollection<KolomWaarde> kolommen { get; set; }
        public Type type { get; set; }
        public ICollection<Rij> waarden { get; set; }

        public UitzendKrachtBesparing()
        {
            kolommen = new List<KolomWaarde>();
            waarden = new List<Rij>();
            type = Type.BAAT;
        }

        public double berekenTotaal()
        {
            throw new NotImplementedException();
        }

        public double getBedrag(int rijNr)
        {
            throw new NotImplementedException();
        }
    }
}
