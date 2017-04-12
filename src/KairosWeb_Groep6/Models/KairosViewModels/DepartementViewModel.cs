using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class DepartementViewModel
    {
        #region Properties
        [HiddenInput]
        public int WerkgeverId { get; set; }

        [HiddenInput]
        public int DepartementId { get; set; }

        [Required]
        [Display(Name = "Naam departement", Prompt = "Naam departement")]
        public string Naam { get; set; }
        #endregion

        #region Constructors
        public DepartementViewModel()
        {
            
        }

        public DepartementViewModel(Departement dep)
        {
            if (dep != null)
            {
                DepartementId = dep.DepartementId;
                Naam = dep.Naam;
            }
        }
        #endregion
    }
}
