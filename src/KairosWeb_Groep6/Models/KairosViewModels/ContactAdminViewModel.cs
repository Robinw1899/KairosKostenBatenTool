using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class ContactAdminViewModel
    {
        [Required]
        public String Onderwerp { get; set; }
        [Required]
        public String Bericht { get; set; }
        /*public Jobcoach Jobcoach { get; set; }*/

        public ContactAdminViewModel(string onderwerp, string bericht)
        {
            
            Onderwerp = onderwerp;
            Bericht = bericht;
        }
    }
}
