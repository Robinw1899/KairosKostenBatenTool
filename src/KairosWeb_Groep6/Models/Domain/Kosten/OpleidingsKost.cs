using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class OpleidingsKost : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; } // kolom = "type"
        public double Bedrag { get; set; }
        #endregion

        #region Constructors
        public OpleidingsKost()
        {
            Type = Type.Kost;
            Soort = Soort.OpleidingsKost;
        }
        #endregion
    }
}
