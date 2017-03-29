﻿using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.LoonKostViewModels
{
    public class LoonkostViewModel
    {
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
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor het aantal uur")]
        public double AantalUrenPerWeek { get; set; }

        [Required(ErrorMessage = "Gelieve het bruto maandloon (fulltime) op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor het bruto maandloon")]
        public double BrutoMaandloonFulltime { get; set; }

        [Required(ErrorMessage = "Gelieve de ondersteuningspremie in te vullen.")]
        [Display(Name = "% Vlaamse ondersteunings-premie")]
        public double Ondersteuningspremie { get; set; }

        [Required(ErrorMessage = "Gelieve het aantal maanden IBO in te vullen.")]
        [Range(0, int.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor het aantal maanden IBO")]
        public int AantalMaandenIBO { get; set; }

        [Required(ErrorMessage = "Gelieve de IBO premie in te vullen.")]
        [Display(Name = "Totale productiviteitspremie IBO")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor de IBO premie")]
        public double IBOPremie { get; set; }

        [Required(ErrorMessage = "Gelieve een doelgroep op te geven.")]
        public Doelgroep? Doelgroep { get; set; }

        public double Bedrag { get; set; }

        public LoonkostViewModel()
        {
            
        }

        public LoonkostViewModel(Loonkost loon)
        {
            Id = loon.Id;
            AantalUrenPerWeek = loon.AantalUrenPerWeek;
            Beschrijving = loon.Beschrijving;
            Bedrag = loon.Bedrag;
            BrutoMaandloonFulltime = loon.BrutoMaandloonFulltime;
            Ondersteuningspremie = loon.Ondersteuningspremie;
            AantalMaandenIBO = loon.AantalMaandenIBO;
            IBOPremie = loon.IBOPremie;
            Doelgroep = loon.Doelgroep;
        }
    }
}
