using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten
{
    public class LoonkostViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Display(Name = "Functie")]
        [Required(ErrorMessage = "Gelieve een (korte) beschrijving van de functie op te geven.")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve een het aantal uren per week op te geven.")]
        [Display(Name = "Aantal werkuren per week", Prompt = "Aantal werkuren per week")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor het aantal uur")]
        public decimal AantalUrenPerWeek { get; set; }

        [Required(ErrorMessage = "Gelieve het bruto maandloon (fulltime) op te geven.")]
        [Display(Name = "Brutomaandloon (fulltime)", Prompt = "Brutomaandloon (fulltime)")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor het bruto maandloon")]
        public string BrutoMaandloonFulltime { get; set; }

        [Required(ErrorMessage = "Gelieve de ondersteuningspremie in te vullen.")]
        [Display(Name = "% Vlaamse ondersteuningspremie")]
        public int Ondersteuningspremie { get; set; }

        [Display(Name = "Aantal maanden IBO", Prompt = "Aantal maanden IBO")]
        [Required(ErrorMessage = "Gelieve het aantal maanden IBO in te vullen.")]
        [Range(0, int.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor het aantal maanden IBO")]
        public int AantalMaandenIBO { get; set; }

        [Required(ErrorMessage = "Gelieve de IBO premie in te vullen.")]
        [Display(Name = "Totale productiviteitspremie IBO")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor de IBO premie")]
        public string IBOPremie { get; set; }

        [Required(ErrorMessage = "Gelieve een doelgroep op te geven.")]
        public DoelgroepSoort DoelgroepSoort { get; set; }

        public Doelgroep Doelgroep { get; set; }

        public string Bedrag { get; set; }
        #endregion

        #region Constructors
        public LoonkostViewModel()
        {
            
        }

        public LoonkostViewModel(Loonkost loon)
        {
            DecimalConverter dc = new DecimalConverter();
            Id = loon.Id;
            AantalUrenPerWeek = loon.AantalUrenPerWeek;
            Beschrijving = loon.Beschrijving;
            Bedrag = dc.ConvertToString(loon.Bedrag);
            BrutoMaandloonFulltime = dc.ConvertToString(loon.BrutoMaandloonFulltime);
            Ondersteuningspremie = (int) loon.Ondersteuningspremie;
            AantalMaandenIBO = loon.AantalMaandenIBO;
            IBOPremie = dc.ConvertToString(loon.IBOPremie);
            
            Doelgroep = loon.Doelgroep;
            if (loon.Doelgroep != null)
            {
                DoelgroepSoort = loon.Doelgroep.Soort;
            }
        }
        #endregion
    }
}
