using System;
using Newtonsoft.Json;

namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class MedewerkerNiveauBaat : KostOfBaat
    {
        #region Properties
        //Beschrijving wordt niet gebruikt!

        public override double Bedrag
        {
            get { return 0; }
            set { }
        }

        
        public double Uren { get; set; }

        
        public double BrutoMaandloonFulltime { get; set; }
        #endregion

        #region Constructors

        public MedewerkerNiveauBaat()
        {
            
        }

       
        public MedewerkerNiveauBaat(bool forJsonOnly)
        {
            
        }

        public MedewerkerNiveauBaat(Soort soort)
        {
            // Soort is ofwel zelfde niveau of hoger niveau
            // De berekening is toch dezelfde
            Type = Type.Baat;
            Soort = soort;
        }
        #endregion

        #region Methods

        public double BerekenTotaleLoonkostPerJaar(double aantalWerkuren, double patronaleBijdrage)
        {
            if (ControleerAlleGegevensAanwezig(aantalWerkuren, patronaleBijdrage))
            {
                double verhoudingUren = Uren / aantalWerkuren;
                double loonMetPatronaleBijdrag = (verhoudingUren * BrutoMaandloonFulltime) * (1 + (patronaleBijdrage / 100));
                return loonMetPatronaleBijdrag * 13.92;
            }

            return 0; // als een gegeven ontbreekt, wordt 0 gereturned
        }

        private bool ControleerAlleGegevensAanwezig(double aantalWerkuren, double patronaleBijdrage)
        {
            if (aantalWerkuren <= 0)
            {
                return false;
            }

            if (patronaleBijdrage <= 0)
            {
                return false;
            }

            if (BrutoMaandloonFulltime <= 0)
            {
                return false;
            }

            if (Uren <= 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
