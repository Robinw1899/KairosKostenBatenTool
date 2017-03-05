﻿namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class Subsidie : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; } // wordt niet gebruikt
        public double Bedrag { get; set; }
        #endregion

        #region Constructors
        public Subsidie()
        {
            Type = Type.Baat;
            Soort = Soort.Subsidie;
        }
        #endregion
    }
}
