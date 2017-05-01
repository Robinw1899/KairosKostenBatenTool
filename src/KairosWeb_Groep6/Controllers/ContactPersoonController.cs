using System;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            if (analyse?.Departement == null)
            {
                TempData["error"] = "U hebt nog geen werkgever geselecteerd, gelieve deze eerst te selecteren";
                return RedirectToAction("Index", "Werkgever");
            }

            try
            {
                int id = analyse.Departement.Werkgever.WerkgeverId;
                ViewData["WerkgeverId"] = id;

                Werkgever werkgever = _werkgeverRepository.GetById(id);

                if(analyse.contactPersooon != null)
                {
                    return RedirectToAction("SelecteerBestaandeContactPersoon", new { WerkgeverId = id, ContactPersoonId = analyse.contactPersooon.ContactPersoonId });
                }

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
            }
            catch
            {
                ModelState.AddModelError("", "Er is al een contactpersoon met dit e-mailadres, gelieve een ander te kiezen");
                return View(model);
            }

            return RedirectToAction("Index", new { id = model.WerkgeverId });
        }
        #endregion

        #region Verwijder
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
                TempData["error"] = "Er is een fout opgetreden bij het proberen verwijderen van de contactpersoon";
            }

            return RedirectToAction("Index", new { id = id });

        }
        #endregion

        #region Bewerk
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
        [HttpPost]
        public IActionResult Bewerk(ContactPersoonViewModel cpViewModel)
        {
            if (ModelState.IsValid)
            {
                Werkgever werkgever = _werkgeverRepository.GetById(cpViewModel.WerkgeverId);
                ContactPersoon cp = werkgever.ContactPersonen.Where(w => w.ContactPersoonId == cpViewModel.PersoonId).FirstOrDefault();

                //controle op een reeds bestaand contacctpersoon            
                cp.Naam = cpViewModel.Naam;
                cp.Voornaam = cpViewModel.Voornaam;
                cp.Emailadres = cpViewModel.Email;

                TempData["message"] = "De contactpersoon " + cpViewModel.Voornaam + " " + cpViewModel.Naam + " is succesvol aangepast";
                return RedirectToAction("Index","ContactPersoon", new { id = cpViewModel.WerkgeverId });
            }

            TempData["error"] = "Er is een fout opgetreden bij het aanpassen van de contactpersoon";
            return View(cpViewModel);          

        }
        #endregion

        #region SelecteerBestaandeContactPersoon
        public IActionResult SelecteerBestaandeContactPersoon(int WerkgeverId, int ContactPersoonId,Analyse analyse)
        {
            Werkgever werkgever = _werkgeverRepository.GetById(WerkgeverId);
            ContactPersoon cp = werkgever.ContactPersonen.Where(w => w.ContactPersoonId == ContactPersoonId).FirstOrDefault();

            analyse.contactPersooon = cp;        

            ContactPersoonViewModel model = new ContactPersoonViewModel(cp, WerkgeverId);
            return View("Bewerk", model);
        }
        #endregion
    }
}
