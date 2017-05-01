using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public readonly IContactPersoonRepository _contactPersoonRepository;
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
                int id = analyse.Departement.Werkgever.WerkgeverId;
                ViewData["WerkgeverId"] = id;
                ViewData["AnalyseId"] = analyse.AnalyseId;

                Werkgever werkgever = _werkgeverRepository.GetById(id);
                if(analyse.contactPersooon != null)
                {
                    return RedirectToAction("Bewerk", new { id = id, cpid = analyse.contactPersooon.ContactPersoonId });
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
                    return RedirectToAction("VoegContactPersoonToe", "ContactPersoon",new {id=werkgever.WerkgeverId });
                }
            }
            catch
            {
                TempData["error"] = "U hebt nog geen werkgever geselecteerd, gelieve deze eerst te selecteren";
                return RedirectToAction("NieuweAnalyse", "Analyse");
            }
        }
        #endregion

        #region AlleContactPersonen
        public IActionResult ViewAlleContactPersonen(Analyse analyse)
        {         
            try
            {
        
                int id = analyse.Departement.Werkgever.WerkgeverId;
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
                    return RedirectToAction("VoegContactPersoonToe", "ContactPersoon", new { id = werkgever.WerkgeverId });
                }
            }
            catch
            {
                TempData["error"] = "U hebt nog geen werkgever geselecteerd, gelieve deze eerst te selecteren";
                return RedirectToAction("NieuweAnalyse", "Analyse");
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

            // controle op een reeds bestaand contactpersoon?
            _contactPersoonRepository.Add(cp);
            werkgever.ContactPersonen.Add(cp);
            _contactPersoonRepository.Save();
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
                _contactPersoonRepository.Remove(cp);
                _werkgeverRepository.Save();
                _departementRepository.Save();             
                _contactPersoonRepository.Save();

            }
            catch (DbUpdateException ex )
            {
                TempData["error"] = ex.Message;
            }
            catch
            {
                TempData["error"] = "Er is een fout opgetreden bij het proberen verwijderen van de contactpersoon";
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

                _werkgeverRepository.Save();
                _departementRepository.Save();
                _contactPersoonRepository.Save();
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
        public IActionResult SelecteerBestaandeContactPersoon(int WerkgeverId, int ContactPersoonId,int AnalyseId)
        {
            
            Werkgever werkgever = _werkgeverRepository.GetById(WerkgeverId);
            ContactPersoon cp = werkgever.ContactPersonen.Where(w => w.ContactPersoonId == ContactPersoonId).FirstOrDefault();

            Analyse analyse = _analyseRepository.GetById(AnalyseId);
            analyse.contactPersooon = cp;
            _analyseRepository.Save();
            _contactPersoonRepository.Save();
            _werkgeverRepository.Save();

            ContactPersoonViewModel model = new ContactPersoonViewModel(cp, WerkgeverId);
            return View("Bewerk", model);
        }

        

       
    }
}
