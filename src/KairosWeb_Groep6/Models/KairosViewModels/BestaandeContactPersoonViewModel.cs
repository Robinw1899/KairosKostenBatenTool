using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class BestaandeContactPersoonViewModel
    {
        [HiddenInput]
        public int WerkgeverId { get; set; }

        public IEnumerable<ContactPersoonViewModel> ContactPersonen { get; set; }
    }
}
