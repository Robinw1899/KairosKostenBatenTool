using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class ResultaatMailViewModel
    {
        #region Properties
        [HiddenInput]
        public int AnalyseId { get; set; }

        [Display(Name = "Naam ontvanger", Prompt = "Naam")]
        [Required(ErrorMessage = "Gelieve de naam van de ontvanger op te geven")]
        public string Naam { get; set; }

        [Display(Name = "Voornaam ontvanger", Prompt = "Voornaam")]
        [Required(ErrorMessage = "Gelieve de voornaam van de ontvanger op te geven")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Gelieve het e-mailadres van de ontvanger op te geven")]
        [Display(Name = "E-mailadres ontvanger", Prompt = "E-mailadres")]
        public string Emailadres { get; set; }

        [Required(ErrorMessage = "Gelieve onderwerp op te geven")]
        [Display(Name = "Onderwerp", Prompt = "Onderwerp")]
        public string Onderwerp { get; set; }
        
        [Required(ErrorMessage = "Gelieve een bericht op te geven")]
        [Display(Name = "Bericht", Prompt = "Typ je bericht hier...")]
        [DataType(DataType.MultilineText)]
        public string Bericht { get; set; }
        #endregion

        #region Constructors
        public ResultaatMailViewModel()
        {
            
        }
        #endregion
    }
}
