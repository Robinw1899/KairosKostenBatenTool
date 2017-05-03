using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class OpmerkingViewModel
    {
        #region Properties
        [Required(ErrorMessage = "Gelieve onderwerp op te geven")]
        public string Onderwerp { get; set; }

        [Required(ErrorMessage = "Gelieve een bericht op te geven")]
        [DataType(DataType.MultilineText)]
        public string Bericht { get; set; }
        #endregion

        #region Constructors
        public OpmerkingViewModel()
        {
            
        }

        public OpmerkingViewModel(string onderwerp, string bericht)
        {          
            Onderwerp = onderwerp;
            Bericht = bericht;
        }
        #endregion
    }
}
