using System;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class BegeleidingsKost : KostOfBaat
    {
        #region Properties

        public double Uren { get; set; }

        public double BrutoMaandloonBegeleider { get; set; }

        public override double Bedrag
        {
            get
            {
                throw new InvalidOperationException("Deze methode is niet voor de klasse BegeleidingsKost");
            }
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

        private bool ControleerGegevensBrutoloonAanwezig(double patronaleBijdrage)
        {
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

        public double GeefJaarbedrag(double patronaleBijdrage)
        {
            if (!ControleerGegevensBrutoloonAanwezig(patronaleBijdrage))
            {
                return 0;
            }

            double totaal = 0;

            double urenVerhouding = Uren / 152;
            double loon = BrutoMaandloonBegeleider * (1 + patronaleBijdrage / 100);
            totaal = urenVerhouding * loon;

            return totaal;
        }
        #endregion
    }
}
