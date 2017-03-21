using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.LoonKostViewModels
{
    public class LoonKostIndexViewModel
    {
        [HiddenInput]
        public int Id { get; set; }//dit toegevoegd
        [Required(ErrorMessage = "Gelieve een (korte) beschrijving van de functie op te geven.")]
        public String Beschrijving { get; set; }
        [Required(ErrorMessage = "Gelieve een het aantal uren per week op te geven.")]
        public double AantalUrenPerWeek { get; set; }

        public double Bedrag // = kolom "totale loonkost eerste jaar"
        {
            get
            {
                return BerekenTotaleLoonkost();
            }
            set { } // setter wordt nooit gebruikt
        }
        [Required(ErrorMessage = "Gelieve het bruto maandloon (fulltime) op te geven.")]
        public double BrutoMaandloonFulltime { get; set; }
        [Required(ErrorMessage = "Gelieve de ondersteuningspremie in te vullen.")]
        public double Ondersteuningspremie { get; set; }
        [Required(ErrorMessage = "Gelieve het aantal maanden IBO in te vullen.")]
        public int AantalMaandenIBO { get; set; }
        [Required(ErrorMessage = "Gelieve de IBO premie in te vullen.")]
        public double IBOPremie { get; set; }
        [Required(ErrorMessage = "Gelieve een doelgroep op te geven.")]
        public Doelgroep? Doelgroep { get; set; }
        public IEnumerable<LoonkostViewModel> ViewModels { get; internal set; }

        public LoonKostIndexViewModel()
        {

        }

        public LoonKostIndexViewModel(Loonkost loon)
        {
            Id = loon.Id;
            Beschrijving = loon.Beschrijving;
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


