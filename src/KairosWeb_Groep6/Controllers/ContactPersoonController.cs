using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Controllers
{
    public class ContactPersoonController : Controller
    {
        #region properties
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;
        #endregion 

        #region constructor
        public ContactPersoonController(
            IAnalyseRepository analyseRepository,
            IDepartementRepository departementenRepository,
            IWerkgeverRepository werkgeverRepository)
        {
            _analyseRepository = analyseRepository;
            _departementRepository = departementenRepository;
            _werkgeverRepository = werkgeverRepository;
        }
        #endregion

        public IActionResult Index(int WerkgeverId)
        {
            Werkgever werkgever = _werkgeverRepository.GetById(WerkgeverId);
            IEnumerable<ContactPersoon> contactpersonen = werkgever.ContactPersonen;


            BestaandeContactPersoonViewModel model = new BestaandeContactPersoonViewModel
            {
                ContactPersonen = contactpersonen.Select(w => new ContactPersoonViewModel(w,WerkgeverId))
                                       .ToList()
            };

            return View(model);

        }      
    }
}
