using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public enum DoelgroepSoort
    {
        [Display(Name = "Wn's < 25 jaar laaggeschoold")]
        LaaggeschooldTot25 = 1550,

        [Display(Name = "Wn's < 25 jaar middengeschoold")]
        MiddengeschooldTot25 = 1000,

        [Display(Name = "Wn's ≥ 55 en < 60 jaar")]
        Tussen55En60 = 1150,

        [Display(Name = "Wns ≥ 60 jaar")]
        Vanaf60 = 1500,

        [Display(Name = "Andere")]
        Andere = 0
    }
}