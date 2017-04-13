using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class LijstDepartementenViewModel
    {
        [HiddenInput]
        public int WerkgeverId { get; set; }

        public IEnumerable<Departement> departementen { get; set; }
       
       public LijstDepartementenViewModel(int id, IEnumerable<Departement> lijst)
        {
            WerkgeverId = id;
            departementen = lijst;
        }

        public LijstDepartementenViewModel(int id)
        {
            WerkgeverId = id;
        }

        public LijstDepartementenViewModel()
        {

        }

    }
}
