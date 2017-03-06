using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class ExtraKost : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; }
        #endregion

        #region Constructors
        public ExtraKost()
        {
            Type = Type.Kost;
            Soort = Soort.ExtraKost;
        }
        #endregion
    }
}
