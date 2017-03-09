namespace KairosWeb_Groep6.Models.Domain.Baten
{
    public class MedewerkerNiveauBaat : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; } // wordt niet gebruikt!

        public double Bedrag
        {
            get
            {
                return BerekenTotaleLoonkostPerJaar();
            }
            set { }
        } // returned berekening
        public double Uren { get; set; }
        public double BrutoMaandloonFulltime { get; set; }
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

        #region Methods

        public double BerekenTotaleLoonkostPerJaar()
        {
            if (ControleerAlleGegevensAanwezig())
            {
                double verhoudingUren = Uren / Werkgever.AantalWerkuren;
                double loonMetPatronaleBijdrag = (verhoudingUren * BrutoMaandloonFulltime) * (1 + Werkgever.PatronaleBijdrage);
                return loonMetPatronaleBijdrag * 13.92;
            }

            return 0; // als een gegeven ontbreekt, wordt 0 gereturned
        }

        private bool ControleerAlleGegevensAanwezig()
        {
            if (Werkgever.AantalWerkuren <= 0)
            {
                return false;
            }

            if (Werkgever.PatronaleBijdrage <= 0)
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
