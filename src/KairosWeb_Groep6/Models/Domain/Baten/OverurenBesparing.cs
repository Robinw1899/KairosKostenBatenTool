using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class OverurenBesparing : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; } // wordt niet gebruikt
        public double Bedrag { get; set; }
        #endregion

        #region Constructors
        public OverurenBesparing()
        {
            Type = Type.Baat;
            Soort = Soort.OverurenBesparing;
        }
        #endregion
    }
}
