using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.LoonKostViewModels
{
    public class LoonKostIndexViewModel
    {
        public int Id { get; set; }//dit toegevoegd
        public double AantalUrenPerWeek { get; set; }

        public double Bedrag // = kolom "totale loonkost eerste jaar"
        {
            get
            {
                return BerekenTotaleLoonkost();
            }
            set { } // setter wordt nooit gebruikt
        }

        public double BrutoMaandloonFulltime { get; set; }

        public double Ondersteuningspremie { get; set; }

        public int AantalMaandenIBO { get; set; }

        public double IBOPremie { get; set; }

        public Doelgroep? Doelgroep { get; set; }
        public IEnumerable<LoonkostViewModel> ViewModels { get; internal set; }

        public LoonKostIndexViewModel()
        {

        }

        public LoonKostIndexViewModel(Loonkost loon)
        {
            AantalUrenPerWeek = loon.AantalUrenPerWeek;
            Bedrag = loon.Bedrag;
            BrutoMaandloonFulltime = loon.BrutoMaandloonFulltime;
            Ondersteuningspremie = loon.Ondersteuningspremie;
            AantalMaandenIBO = loon.AantalMaandenIBO;
            IBOPremie = loon.IBOPremie;
            Doelgroep = loon.Doelgroep;

        }
    }
}


