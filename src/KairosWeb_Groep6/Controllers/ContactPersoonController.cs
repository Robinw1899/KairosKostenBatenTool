using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    public class ContactPersoonController : Controller
    {
        #region Properties
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;
        private readonly IContactPersoonRepository _contactPersoonRepository;
        #endregion 

        #region Constructor
        public ContactPersoonController(
            IAnalyseRepository analyseRepository,
            IDepartementRepository departementenRepository,
            IWerkgeverRepository werkgeverRepository,
            IContactPersoonRepository contactPersoonRepository)
        {
            _analyseRepository = analyseRepository;
            _departementRepository = departementenRepository;
            _werkgeverRepository = werkgeverRepository;
            _contactPersoonRepository = contactPersoonRepository;
        }
        #endregion

        #region Index
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Index(Analyse analyse)
        {
            try
            {
                if (analyse.Departement == null)
                {
                    TempData["error"] = "U hebt nog geen werkgever geselecteerd, gelieve deze eerst te selecteren";
                    return RedirectToAction("Index", "Werkgever");
                }

                ViewData["analyseId"] = analyse.AnalyseId;

                int id = analyse.Departement.Werkgever.WerkgeverId;
                ViewData["WerkgeverId"] = id;

                Werkgever werkgever = _werkgeverRepository.GetById(id);

                analyse.Departement = _departementRepository.GetById(analyse.Departement.DepartementId);

                if(analyse.Departement?.ContactPersoon != null)
                {
                    ContactPersoonViewModel model = new ContactPersoonViewModel(analyse.Departement?.ContactPersoon, 
                        analyse.Departement.Werkgever.WerkgeverId);
                    return View("Index", model);
                }
                else
                {
                    TempData["error"] = "Er is nog geen contactpersoon, voeg hier eventueel een contactpersoon toe";
                    return RedirectToAction("VoegContactPersoonToe", new { id = werkgever.WerkgeverId });
                }
            }
            catch
            {
                TempData["error"] = "U hebt nog geen werkgever geselecteerd, gelieve deze eerst te selecteren";
                return RedirectToAction("Index", "Werkgever");
            }
        }
        #endregion

        #region VoegToe
        public IActionResult VoegContactPersoonToe(int id)
        {
            ContactPersoonViewModel model = new ContactPersoonViewModel {WerkgeverId = id};

            return View("Index", model);
        }

        [HttpPost]
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult VoegContactPersoonToe(Analyse analyse, ContactPersoonViewModel model)
        {
            try
            {
                analyse.Departement = _departementRepository.GetById(analyse.Departement.DepartementId);
                ContactPersoon cp = new ContactPersoon(model.Voornaam, model.Naam, model.Email);

                analyse.Departement.ContactPersoon = cp;
                _departementRepository.Save();
                _analyseRepository.Save();

                TempData["message"] = "De contactpersoon " + cp.Voornaam + " " + cp.Naam + " is succesvol toegevoegd";
            }
            catch
            {
                ModelState.AddModelError("", "Er is al een contactpersoon met dit e-mailadres, gelieve een ander te kiezen");
                return View("Index", model);
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Opslaan
        [HttpPost]
        public IActionResult Opslaan(ContactPersoonViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContactPersoon cp = _contactPersoonRepository
                        .GetById(model.PersoonId);

                    if (cp != null)
                    {
                        cp.Naam = model.Naam;
                        cp.Voornaam = model.Voornaam;
                        cp.Emailadres = model.Email;

                        _contactPersoonRepository.Save();

                        TempData["message"] = "De contactpersoon " + model.Voornaam + " " + model.Naam +
                                              " is succesvol opgeslaan";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "Er ging iets mis tijdens het opslaan, probeer later opnieuw";
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Er is al een contactpersoon met die e-mailadres");
            }

            // als we hier komen, moet het formulier nog eens getoond worden
            return View("Index", model);
        }
        #endregion

        #region Verwijder
        public IActionResult VerwijderContactpersoon(int id, int werkgeverid)
        {
            // id is het id van de contactpersoon
            try
            {
                ContactPersoon cp = _contactPersoonRepository.GetById(id);

                ViewData["contactPersoonId"] = id;
                ViewData["werkgeverId"] = werkgeverid;
                ViewData["contactpersoon"] = cp.Voornaam + " " + cp.Naam;
            }
            catch
            {
                TempData["error"] = "Er is een fout opgetreden tijdens het voorbereiden van het verwijderen, probeer later opnieuw";
                return RedirectToAction("Index");
            }

            return View("Verwijder");
        }

        [ActionName("Verwijder")]
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult VerwijderBevestigd(int id, int werkgeverid, Analyse analyse)
        {
            // id is het id van de contactpersoon

            try
            {
                // contactpersoon verwijderen van Departement
                analyse.Departement.ContactPersoon = null;
                ContactPersoon cp = _contactPersoonRepository.GetById(id);

                // contactpersoon verwijderen uit repo
                _contactPersoonRepository.Remove(cp);

                // alles opslaan
                _contactPersoonRepository.Save();
                _analyseRepository.Save();

                TempData["message"] = "De contactpersoon " + cp.Voornaam + " " + cp.Naam + " is succesvol verwijderd";
            }
            catch
            {
                TempData["error"] = "Er is een fout opgetreden tijdens het verwijderen van de contactpersoon, probeer later opnieuw";
            }

            return RedirectToAction("Index");

        }
        #endregion

        #region SelecteerContactPersoon
        //public IActionResult SelecteerHoofdContactPersoon(int werkgeverId, int contactPersoonId)
        //{
        //    return RedirectToAction("SelecteerContactPersoon", new
        //    {
        //        werkgeverId,
        //        contactPersoonId
        //    });
        //}

        //[ServiceFilter(typeof(AnalyseFilter))]
        //public IActionResult SelecteerContactPersoon(int werkgeverId, int contactPersoonId, Analyse analyse)
        //{
        //    try
        //    {
        //        Werkgever werkgever = _werkgeverRepository.GetById(werkgeverId);
        //        ContactPersoon cp = werkgever
        //            .ContactPersonen
        //            .FirstOrDefault(c => c.ContactPersoonId == contactPersoonId);

        //        if (cp != null)
        //        {
        //            analyse.ContactPersooon = cp;
        //            _analyseRepository.Save();
        //        }
        //        else
        //        {
        //            TempData["error"] = "Deze contactpersoon bestaat niet, kies een andere";
        //        }
        //    }
        //    catch
        //    {
        //        TempData["error"] = "Er ging iets mis tijdens het ophalen en instellen van de contactpersoon";
        //    }

        //    return RedirectToAction("Index");
        //}
        #endregion
    }
}
