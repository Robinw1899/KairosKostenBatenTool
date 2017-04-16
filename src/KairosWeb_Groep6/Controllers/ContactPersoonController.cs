using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace KairosWeb_Groep6.Controllers
{
    public class ContactPersoonController : Controller
    {
        #region Properties
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;
        #endregion 

        #region Constructor
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

        #region Index
        public IActionResult Index(int id)
        {
            try
            {
                ViewData["WerkgeverId"] = id;

                Werkgever werkgever = _werkgeverRepository.GetById(id);

                if (werkgever.ContactPersonen.Any())
                {
                    // als er contactpersonen zijn
                    IEnumerable<ContactPersoon> contactpersonen = werkgever.ContactPersonen;

                    IEnumerable<ContactPersoonViewModel> viewModels
                        = contactpersonen
                            .Select(w => new ContactPersoonViewModel(w, id))
                            .ToList();

                    return View(viewModels);
                }
                else
                {
                    TempData["error"] = "Er is nog geen contactpersoon, voeg hier eventueel een contactpersoon toe";
                    return RedirectToAction("VoegContactPersoonToe", "Werkgever");
                }
            }
            catch
            {
                TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
                return RedirectToAction("VoegContactPersoonToe", "Werkgever");
            }
        }
        #endregion
    }
}
