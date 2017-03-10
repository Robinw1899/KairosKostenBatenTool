using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class OpmerkingViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Onderwerp { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Bericht { get; set; }
        [HiddenInput]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public OpmerkingViewModel()
        {
            
        }

        public OpmerkingViewModel(Jobcoach gebruiker, string onderwerp, string bericht)
        {
            Email = gebruiker.Emailadres;
            Onderwerp = onderwerp;
            Bericht = bericht;
        }
    }
}
