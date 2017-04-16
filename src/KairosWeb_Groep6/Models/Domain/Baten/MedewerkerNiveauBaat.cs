namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class MedewerkerNiveauBaat : KostOfBaat
    {
        #region Properties
        //Beschrijving wordt niet gebruikt!

        public override decimal Bedrag
        {
            get { return 0; }
            set { }
        }

        
        public decimal Uren { get; set; }

        
        public decimal BrutoMaandloonFulltime { get; set; }
        #endregion

        #region Constructors

        public MedewerkerNiveauBaat()
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

        #region Controleermethoden
        private bool ControleerAlleGegevensAanwezig(decimal aantalWerkuren, decimal patronaleBijdrage)
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

        #region Methods
        public decimal BerekenTotaleLoonkostPerJaar(decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            if (ControleerAlleGegevensAanwezig(aantalWerkuren, patronaleBijdrage))
            {
                decimal verhoudingUren = Uren / aantalWerkuren;
                decimal loonMetPatronaleBijdrag = (verhoudingUren * BrutoMaandloonFulltime) * (1 + (patronaleBijdrage / 100));
                return loonMetPatronaleBijdrag * 13.92M;
            }

            return 0; // als een gegeven ontbreekt, wordt 0 gereturned
        }
        #endregion
    }
}
