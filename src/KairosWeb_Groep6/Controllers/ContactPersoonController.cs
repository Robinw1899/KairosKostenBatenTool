using KairosWeb_Groep6.Filters;
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
                ViewData["analyseId"] = analyse.AnalyseId;

                if (analyse.Departement == null)
                {
                    TempData["error"] = "U hebt nog geen werkgever geselecteerd, gelieve deze eerst te selecteren";
                    return RedirectToAction("Index", "Werkgever");
                }

                int id = analyse.Departement.Werkgever.WerkgeverId;
                ViewData["WerkgeverId"] = id;

                Werkgever werkgever = _werkgeverRepository.GetById(id);

                if(analyse.ContactPersooon != null)
                {
                    ContactPersoonViewModel model = new ContactPersoonViewModel(analyse.ContactPersooon, 
                        analyse.Departement.Werkgever.WerkgeverId);
                    return View("Bewerk", model);
                }

                if (werkgever.ContactPersonen.Any())
                {
                    // doorsturen naar andere action die dit zal afhandelen
                    return RedirectToAction("ToonAlleContactPersonen");
                }
                else
                {
                    TempData["error"] = "Er is nog geen contactpersoon, voeg hier eventueel een contactpersoon toe";
                    return RedirectToAction("VoegContactPersoonToe", "ContactPersoon",new { id = werkgever.WerkgeverId });
                }
            }
            catch
            {
                TempData["error"] = "U hebt nog geen werkgever geselecteerd, gelieve deze eerst te selecteren";
                return RedirectToAction("Index", "Werkgever");
            }
        }
        #endregion

        #region ToonAlleContactPersonen
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult ToonAlleContactPersonen(Analyse analyse)
        {
            int werkgeverId = analyse.Departement.Werkgever.WerkgeverId;
            Werkgever werkgever = _werkgeverRepository.GetById(werkgeverId);

            IEnumerable<ContactPersoon> contactpersonen = werkgever.ContactPersonen;

            IEnumerable<ContactPersoonViewModel> viewModels
                = contactpersonen
                    .Select(w => new ContactPersoonViewModel(w, werkgever.WerkgeverId))
                    .ToList();

            ViewData["WerkgeverId"] = werkgeverId;

            return View("Index", viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegContactPersoonToe(int id)
        {
            ContactPersoonViewModel model = new ContactPersoonViewModel {WerkgeverId = id};

            return View(model);
        }

        [HttpPost]
        public IActionResult VoegContactPersoonToe(ContactPersoonViewModel model)
        {
            Werkgever werkgever = _werkgeverRepository.GetById(model.WerkgeverId);
            ContactPersoon cp = new ContactPersoon(model.Voornaam, model.Naam, model.Email);

            try
            {
                _contactPersoonRepository.Add(cp);
                _contactPersoonRepository.Save();
                werkgever.ContactPersonen.Add(cp);
                _werkgeverRepository.Save();

                TempData["message"] = "De contactpersoon " + cp.Voornaam + " " + cp.Naam + " is succesvol toegevoegd";
            }
            catch
            {
                ModelState.AddModelError("", "Er is al een contactpersoon met dit e-mailadres, gelieve een ander te kiezen");
                return View(model);
            }

            return RedirectToAction("ToonAlleContactPersonen");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(int id, int cpid)
        {
            // id = werkgeverid
            // cpid = contactpersoonid
            ContactPersoonViewModel model;

            try
            {
                Werkgever werkgever = _werkgeverRepository.GetById(id);

                ContactPersoon cp = werkgever.ContactPersonen
                    .SingleOrDefault(w => w.ContactPersoonId == cpid);

                model = new ContactPersoonViewModel(cp, id);

                return View(model);
            }
            catch
            {
                TempData["error"] = "Er is een fout opgetreden bij het proberen verwijderen van de contact persoon";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(ContactPersoonViewModel model)
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

                        TempData["message"] = "De contactpersoon " + model.Voornaam + " " + model.Naam + " is succesvol opgeslaan";
                        return RedirectToAction("ToonAlleContactPersonen");
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Er is al een contactpersoon met die e-mailadres");
            }

            // als we hier komen, moet het formulier nog eens getoond worden
            return View(model);
        }
        #endregion

        #region Verwijder
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult VerwijderContactpersoon(int id, int werkgeverid, Analyse analyse)
        {
            // id is het id van de contactpersoon
            try
            {
                ContactPersoon cp = _contactPersoonRepository.GetById(id);

                if (analyse.ContactPersooon != null && analyse.ContactPersooon.Equals(cp))
                {
                    TempData["error"] = "Deze contactpersoon is ingesteld als contactpersoon voor deze analyse " +
                                          "en kan dus niet verwijderd worden. Kies eerst een andere " +
                                          "contactpersoon voor deze analyse.";

                    return RedirectToAction("ToonAlleContactPersonen");
                }

                ViewData["contactPersoonId"] = id;
                ViewData["werkgeverId"] = werkgeverid;
                ViewData["contactpersoon"] = cp.Voornaam + " " + cp.Naam;
            }
            catch
            {
                TempData["error"] = "Er is een fout opgetreden tijdens het voorbereiden van het verwijderen, probeer later opnieuw";
                return RedirectToAction("ToonAlleContactPersonen");
            }

            return View("Verwijder");
        }

        [ActionName("Verwijder")]
        public IActionResult VerwijderBevestigd(int id, int werkgeverid)
        {
            // id is het id van de contactpersoon

            try
            {
                ContactPersoon cp = _contactPersoonRepository.GetById(id);
                Werkgever werkgever = _werkgeverRepository.GetById(werkgeverid);

                // eerst bij de werkgever verwijderen
                werkgever.ContactPersonen.Remove(cp);
                _werkgeverRepository.Save();

                // nadien de contactpersoon
                _contactPersoonRepository.Remove(cp);
                _contactPersoonRepository.Save();

                TempData["message"] = "De contactpersoon " + cp.Voornaam + " " + cp.Naam + " is succesvol verwijderd";
            }
            catch
            {
                TempData["error"] = "Er is een fout opgetreden tijdens het verwijderen van de contactpersoon, probeer later opnieuw";
            }

            return RedirectToAction("ToonAlleContactPersonen");

        }
        #endregion

        #region SelecteerContactPersoon
        public IActionResult SelecteerHoofdContactPersoon(int werkgeverId, int contactPersoonId)
        {
            try
            {
                return RedirectToAction("SelecteerContactPersoon", new
                {
                    werkgeverId,
                    contactPersoonId
                });
            }
            catch
            {
                TempData["error"] = "Er ging iets mis tijdens het ophalen van de huidige analyse";
            }

            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult SelecteerContactPersoon(int werkgeverId, int contactPersoonId, Analyse analyse)
        {
            try
            {
                Werkgever werkgever = _werkgeverRepository.GetById(werkgeverId);
                ContactPersoon cp = werkgever
                    .ContactPersonen
                    .FirstOrDefault(c => c.ContactPersoonId == contactPersoonId);

                if (cp != null)
                {
                    analyse.ContactPersooon = cp;
                    _analyseRepository.Save();
                }
                else
                {
                    TempData["error"] = "Deze contactpersoon bestaat niet, kies een andere";

                }
            }
            catch
            {
                TempData["error"] = "Er ging iets mis tijdens het ophalen en instellen van de contactpersoon";
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
