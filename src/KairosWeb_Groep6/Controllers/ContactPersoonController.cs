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
        #region Contactpersonen
        public IActionResult VoegContactPersoonToe(int id)
        {
            ContactPersoonViewModel model = new ContactPersoonViewModel();
            model.WerkgeverId = id;
            return View(model);
        }
        [HttpPost]
        public IActionResult VoegContactPersoonToe(ContactPersoonViewModel cpViewModel)
        {
            Werkgever werkgever = _werkgeverRepository.GetById(cpViewModel.WerkgeverId);
            ContactPersoon cp = new ContactPersoon(cpViewModel.Voornaam, cpViewModel.Naam, cpViewModel.Email);

            werkgever.ContactPersonen.Add(cp);
            _werkgeverRepository.Save();
            _departementRepository.Save();

            return RedirectToAction("Index", new { id = cpViewModel.WerkgeverId });
        }
        #endregion
        

        public IActionResult Verwijder(int id,int cpid)
        {
            try
            {
                Werkgever werkgever = _werkgeverRepository.GetById(id);
                ContactPersoon cp = werkgever.ContactPersonen.Where(w => w.ContactPersoonId == cpid).SingleOrDefault();

                werkgever.ContactPersonen.Remove(cp);
                _werkgeverRepository.Save();

            }
            catch
            {
                TempData["error"] = "Er is een fout opgetreden bij het proberen verwijderen van de contact persoon";
            }

            return RedirectToAction("Index", new { id = id });

        }


        public IActionResult Bewerk(int id,int cpid)
        {
            ContactPersoonViewModel model;
            try
            {
                Werkgever werkgever = _werkgeverRepository.GetById(id);

                ContactPersoon cp = werkgever.ContactPersonen.Where(w => w.ContactPersoonId == cpid).SingleOrDefault();

                 model = new ContactPersoonViewModel(cp, id);

                return View(model);
            }
            catch
            {
                TempData["error"] = "Er is een fout opgetreden bij het proberen verwijderen van de contact persoon";
            }

            return RedirectToAction("Index", id);
           
        }
    }
}
