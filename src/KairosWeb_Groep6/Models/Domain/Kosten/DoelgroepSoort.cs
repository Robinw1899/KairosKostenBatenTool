using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public enum DoelgroepSoort
    {
        [Display(Name = "Wn's < 25 jaar laaggeschoold")]
        LaaggeschooldTot25 = 1,

        [Display(Name = "Wn's < 25 jaar middengeschoold")]
        MiddengeschooldTot25 = 2,

        [Display(Name = "Wn's ≥ 55 en < 60 jaar")]
        Tussen55En60 = 3,

        [Display(Name = "Wns ≥ 60 jaar")]
        Vanaf60 = 4,

        [Display(Name = "Andere")]
        Andere = 5
    }
}