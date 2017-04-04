using KairosWeb_Groep6.Models.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public OpmerkingViewModel()
        {
            
        }

        public OpmerkingViewModel(string onderwerp, string bericht)
        {          
            Onderwerp = onderwerp;
            Bericht = bericht;
        }
    }
}
