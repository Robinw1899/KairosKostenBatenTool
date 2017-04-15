namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class BegeleidingsKost : KostOfBaat
    {
        #region Properties

        public decimal Uren { get; set; }

        public decimal BrutoMaandloonBegeleider { get; set; }

        public override decimal Bedrag
        {
            get { return 0; }
            set { }
        }

        // bedrag = kolom 'jaarbedrag' uit Excel
        #endregion

        #region Constructors
        public BegeleidingsKost()
        {
            Type = Type.Kost;
            Soort = Soort.BegeleidingsKost;
        }
        #endregion

        #region Controleermethoden

        private bool ControleerGegevensBrutoloonAanwezig(decimal patronaleBijdrage)
        {
            if (patronaleBijdrage <= 0)
            {
                return false;
            }

            if (BrutoMaandloonBegeleider <= 0)
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

        public decimal GeefJaarbedrag(decimal patronaleBijdrage)
        {
            if (!ControleerGegevensBrutoloonAanwezig(patronaleBijdrage))
            {
                return 0M;
            }

            decimal totaal;

            decimal urenVerhouding = Uren / 152;
            decimal loon = BrutoMaandloonBegeleider * (1 + patronaleBijdrage / 100);
            totaal = urenVerhouding * loon;

            return totaal;
        }
        #endregion
    }
}
