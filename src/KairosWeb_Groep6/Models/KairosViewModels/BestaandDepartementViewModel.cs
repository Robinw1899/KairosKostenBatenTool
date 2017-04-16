using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class BestaandDepartementViewModel
    {
        #region Properties
        [HiddenInput]
        public int WerkgeverId { get; set; }

        public IEnumerable<DepartementViewModel> Departementen { get; set; }
        #endregion
    }
}
