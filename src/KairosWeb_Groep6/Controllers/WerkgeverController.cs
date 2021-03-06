﻿using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class WerkgeverController : Controller
    {
        #region Properties
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly IJobcoachRepository _jobcoachRepository;
        #endregion

        #region Constructors
        public WerkgeverController(
            IAnalyseRepository analyseRepository,
            IDepartementRepository departementenRepository,
            IWerkgeverRepository werkgeverRepository,
            IExceptionLogRepository exceptionLogRepository,
            IJobcoachRepository jobcoachRepository)
        {
            _analyseRepository = analyseRepository;
            _departementRepository = departementenRepository;
            _werkgeverRepository = werkgeverRepository;
            _exceptionLogRepository = exceptionLogRepository;
            _jobcoachRepository = jobcoachRepository;
        }
        #endregion

        #region Index
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Index(Analyse analyse)
        {
            if (analyse.Departement == null || analyse.Departement.Naam.Length == 0)
            {
                // er is nog geen werkgever, vragen om een werkgever te selecteren
                return RedirectToAction("SelecteerWerkgever");
            }

            WerkgeverViewModel model = new WerkgeverViewModel(analyse.Departement);

            return View(model);
        }
        #endregion

        #region Opslaan
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Opslaan(Analyse analyse, WerkgeverViewModel model)
        {
            try
            {
                Departement departement = _departementRepository.GetById(model.DepartementId);
                Werkgever werkgever = departement.Werkgever;
                DecimalConverter dc = new DecimalConverter();
                analyse = _analyseRepository.GetByIdAll(analyse.AnalyseId);
                // werkgever instellen
                Werkgever nieuweWerkgever = new Werkgever(model.Naam, model.Straat, model.Nummer ?? 0, model.Bus,
                    model.Postcode, model.Gemeente, dc.ConvertToDecimal(model.AantalWerkuren), dc.ConvertToDecimal(model.PatronaleBijdrage));

                // departement instellen
                if (departement != null && !string.Equals(model.Departement, departement.Naam))
                {
                    // de jobcoach heeft de departementsnaam gewijzigd,
                    // dus we maken een nieuw departement aan
                    departement = new Departement(model.Departement){ Werkgever = werkgever};
                    werkgever.Departementen.Add(departement);
                }

                if (werkgever != null && !nieuweWerkgever.Equals(werkgever))
                {
                    werkgever.Departementen.Remove(departement);
                    departement = new Departement(model.Departement) { Werkgever = nieuweWerkgever };
                    nieuweWerkgever.Departementen.Add(departement);
                }
                else
                {
                    departement.Werkgever = werkgever;
                }

                // instellen in de analyse
                analyse.Departement = departement;

                // alles opslaan
                _departementRepository.Save();
                _analyseRepository.Save();

                TempData["message"] = "De werkgever is succesvol opgeslaan";

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    TempData["error"] = e.Message;
                }
                else
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "Opslaan "));
                    _exceptionLogRepository.Save();
                    TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw";

                }
            }

            return View("Index", model);
        }
        #endregion

        #region SelecteerWerkgever
        public IActionResult SelecteerWerkgever()
        {
            return View("SelecteerWerkgever");
        }
        #endregion

        #region Nieuwe werkgever
        public IActionResult NieuweWerkgever()
        {
            // model aanmaken
            WerkgeverViewModel model = new WerkgeverViewModel{PatronaleBijdrage = "35"};

            // view returnen
            return View(model);
        }

        [HttpPost]
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult NieuweWerkgever(Analyse analyse, WerkgeverViewModel model)
        {
            try
            {
                Departement departement = _departementRepository.GetByName(model.Departement);
                analyse = _analyseRepository.GetByIdAll(analyse.AnalyseId);
                DecimalConverter dc = new DecimalConverter();
                // de werkgever al aanmaken, zodat straks de controle kan gebeuren
                Werkgever werkgever = new Werkgever
                {  // nieuwe werkgever aanmaken
                    Naam = model.Naam,
                    Postcode = model.Postcode,
                    Gemeente = model.Gemeente,
                    AantalWerkuren = dc.ConvertToDecimal(model.AantalWerkuren),
                    PatronaleBijdrage = dc.ConvertToDecimal(model.PatronaleBijdrage)
                };

                // straat en nummer zijn niet verplicht,
                // maar als ze ingevuld zijn, instellen in de werkgever
                werkgever.Straat = model.Straat;
                werkgever.Nummer = model.Nummer;
                werkgever.Bus = model.Bus;

                bool result = ControleerBestaandDepartement(departement, werkgever);

                if (result)
                {
                    TempData["Error"] = "De werkgever " + model.Naam + " met als departement " + model.Departement + " bestaat al.";

                    // terugsturen naar het formulier
                    return View("NieuweWerkgever", model);
                }

                // anders maken we een nieuw departement aan
                departement = new Departement(model.Departement) { Werkgever = werkgever };

                werkgever.Departementen.Add(departement);

                // alles instellen
                _departementRepository.Add(departement);
                analyse.Departement = departement;

                // alles opslaan
                _departementRepository.Save();
                _analyseRepository.Save();

                TempData["message"] = "De werkgever is succesvol toegevoegd";

                return RedirectToAction("Index", "ContactPersoon");
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    TempData["error"] = e.Message;
                }
                else
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "NieuweWerkgever -- POST --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw";

                }
            }

            return View("NieuweWerkgever", model);
        }
        #endregion

        #region Bestaande werkgever
        public IActionResult BestaandeWerkgever()
        {
            try
            {
                BestaandeWerkgeverViewModel model = new BestaandeWerkgeverViewModel();
                return View(model);               
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "BestaandeWerkgever -- GET --"));
                _exceptionLogRepository.Save();
                TempData["Error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(AnalyseFilter))]
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult SelecteerBestaandeWerkgever(Jobcoach jobcoach, Analyse analyse, int id, int werkgeverid)
        { // id is het id van het departement dat geselecteerd werd
            try
            {
                List<Departement> departementenVanJobcoach = _jobcoachRepository.GetDepartementenVanJobcoach(jobcoach);
                Departement departement = departementenVanJobcoach.SingleOrDefault(d => d.DepartementId == id);
                analyse = _analyseRepository.GetByIdAll(analyse.AnalyseId);
                if (departement == null)
                {
                    TempData["error"] = "U heeft geen toegang tot dit departement, kies enkel degene die u hieronder ziet.";
                    return RedirectToAction("BestaandeWerkgever", "", null);
                }

                analyse.Departement = departement;

                _analyseRepository.Save();

                if (HttpContext != null)
                {
                    AnalyseFilter.SetAnalyseInSession(HttpContext, analyse);
                }

                return RedirectToAction("Index", "ContactPersoon");
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "BestaandeWerkgever -- POST --"));
                _exceptionLogRepository.Save();
                TempData["Error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
            }

            return RedirectToAction("BestaandeWerkgever","",null);
        }
        #endregion

        #region Zoekmethoden
        public IActionResult ZoekDepartementen(int id, string naam)
        { // id is id van werkgever
            try
            {
                IEnumerable<Departement> departementen = _departementRepository.GetAllVanWerkgever(id);

                if (naam != null)
                {
                    departementen = departementen
                        .Where(d => d.Naam.IndexOf(naam, StringComparison.OrdinalIgnoreCase) >= 0); // departementen filteren
                }

                var models = departementen.Select(d => new DepartementViewModel(d)
                {
                    WerkgeverId = id
                });

                return PartialView("_Departementen", models);
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "ZoekDepartement"));
                _exceptionLogRepository.Save();
                TempData["Error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
            }

            return RedirectToAction("BestaandDepartement");
        }


        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult ZoekWerkgever(Jobcoach jobcoach, BestaandeWerkgeverViewModel model, string naam = "")
        {
            try
            {              
                IEnumerable<Werkgever> werkgevers = new List<Werkgever>() ;

                if (naam != null && !naam.Equals(""))
                    werkgevers = _werkgeverRepository.GetByName(jobcoach, naam);
                else
                    werkgevers = _jobcoachRepository.GetWerkgeversVanJobcoach(jobcoach);


                 model = new BestaandeWerkgeverViewModel
                {
                    Werkgevers = werkgevers.Select(w => new WerkgeverViewModel(w))
                    .ToList(),
                    FirstLoad = false
                };
           
                return PartialView("_Werkgevers", model);
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "ZoekWerkgever"));
                _exceptionLogRepository.Save();
                TempData["Error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
            }

            return RedirectToAction("BestaandeWerkgever");
        }
        #endregion

        #region Bestaand departement
        public IActionResult BestaandDepartement(int id)
        {// id is is het id van de werkgever
            try
            {
                IEnumerable<Departement> departementen = _departementRepository.GetAllVanWerkgever(id);

                BestaandDepartementViewModel model = new BestaandDepartementViewModel
                {
                    WerkgeverId = id,
                    Departementen = departementen
                        .Select(d => new DepartementViewModel(d)
                        {
                            WerkgeverId = id
                        })
                        .AsEnumerable()
                };

                return View(model);
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "BestaandDepartement"));
                _exceptionLogRepository.Save();
                TempData["Error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
            }

            return RedirectToAction("BestaandeWerkgever");
        }
        #endregion

        #region ToonAlles
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult ToonAlles(Jobcoach jobcoach)
        {
            BestaandeWerkgeverViewModel model = new BestaandeWerkgeverViewModel()
            {
                Werkgevers = _jobcoachRepository
                .GetWerkgeversVanJobcoach(jobcoach)
                .Select(w => new WerkgeverViewModel(w))
                .ToList(),
                FirstLoad = false              
            };

            return PartialView("_Werkgevers", model);
        }
        #endregion

        #region Nieuw departement
        public IActionResult NieuwDepartement(int id)
        {
            try
            {
                // id is het id van de werkgever
                // werkgever ophalen
                Werkgever werkgever = _werkgeverRepository.GetById(id);

                // viewmodel aanmaken
                WerkgeverViewModel model = new WerkgeverViewModel(werkgever);

                return View(model);
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "NieuwDepartement -- GET --"));
                _exceptionLogRepository.Save();
                TempData["Error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
            }

            return RedirectToAction("BestaandeWerkgever");
        }

        [HttpPost]
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult NieuwDepartement(Analyse analyse, WerkgeverViewModel model)
        {
            try
            {
                Departement departement = _departementRepository.GetByName(model.Departement);
                DecimalConverter dc = new DecimalConverter();
                Werkgever werkgever = new Werkgever
                {  // nieuwe werkgever aanmaken
                    Naam = model.Naam,
                    Postcode = model.Postcode,
                    Gemeente = model.Gemeente,
                    AantalWerkuren = dc.ConvertToDecimal(model.AantalWerkuren),
                    PatronaleBijdrage = dc.ConvertToDecimal(model.PatronaleBijdrage),
                    // straat en nummer zijn niet verplicht,
                    // maar als ze ingevuld zijn, instellen in de werkgever
                    Straat = model.Straat,
                    Nummer = model.Nummer,
                    Bus = model.Bus
                };

                bool result = ControleerBestaandDepartement(departement, werkgever);

                if (result)
                {
                    TempData["Error"] = "De werkgever " + model.Naam + " met als departement " + model.Departement + " bestaat al.";

                    // terugsturen naar het formulier
                    return View("NieuwDepartement", model);
                }

                departement = new Departement(model.Departement);
                werkgever.Departementen.Add(departement);

                werkgever = _werkgeverRepository.GetById(model.WerkgeverId);

                departement.Werkgever = werkgever;

                // alles instellen
                _departementRepository.Add(departement);
                analyse.Departement = departement;

                // alles opslaan
                _departementRepository.Save();
                _analyseRepository.Save();

                TempData["message"] = "Het nieuwe departement is succesvol toegevoegd";

                return RedirectToAction("Index", "ContactPersoon");
            }        
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    TempData["error"] = e.Message;
                }
                else
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "Werkgever", "NieuwDepartement -- POST --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
                
                }
            }

            return View(model);
        }
        #endregion

        #region Helpers
        private bool ControleerBestaandDepartement(Departement departement, Werkgever werkgever)
        {
            if (departement != null)
            {
                // het departement bestaat al, kijken of de werkgever ook al bestaat
                Werkgever other = departement.Werkgever;

                if (string.Equals(werkgever.Naam, other.Naam)
                    && string.Equals(werkgever.Straat, other.Straat)
                    && werkgever.Nummer == other.Nummer
                    && werkgever.Postcode == other.Postcode
                    && string.Equals(werkgever.Gemeente, other.Gemeente)
                    && werkgever.AantalWerkuren.Equals(other.AantalWerkuren))
                {
                    // beiden bestaan al
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
