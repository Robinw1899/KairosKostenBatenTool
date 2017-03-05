using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class MedewerkerNiveau : KostOfBaat
    {
        public ICollection<KolomWaarde> kolommen
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Niveau Niveau { get; set; }

        public Type type
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<Rij> waarden
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
