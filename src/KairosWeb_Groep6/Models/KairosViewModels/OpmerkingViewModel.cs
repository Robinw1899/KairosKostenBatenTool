using KairosWeb_Groep6.Models.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class OpmerkingViewModel
    {
        #region Properties
        [Required]
        [DataType(DataType.Text)]
        public string Onderwerp { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Bericht { get; set; }
        #endregion

        #region Constructors
        public OpmerkingViewModel()
        {
            Onderwerp = "";
            Bericht = "";
        }

        public OpmerkingViewModel(string onderwerp, string bericht)
        {          
            Onderwerp = onderwerp;
            Bericht = bericht;
        }
        #endregion
    }
}
