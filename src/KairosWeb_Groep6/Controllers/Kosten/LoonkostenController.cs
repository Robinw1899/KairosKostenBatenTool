using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [AutoValidateAntiforgeryToken]
    public class LoonkostenController : Controller
    {
        #region Properties
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IDoelgroepRepository _doelgroepRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        #endregion

        #region Constructors
        public LoonkostenController(IAnalyseRepository analyseRepository,
            IDoelgroepRepository doelgroepRepository, IExceptionLogRepository exceptionLogRepository)
        {
            _analyseRepository = analyseRepository;
            _doelgroepRepository = doelgroepRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }
        #endregion

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            analyse.UpdateTotalen(_analyseRepository);

            PlaatsTotaalInViewData(analyse);

            return View(MaakModel(analyse));
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            LoonkostFormViewModel model = new LoonkostFormViewModel(_doelgroepRepository.GetAll());
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, LoonkostFormViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DecimalConverter dc = new DecimalConverter();
                    Loonkost kost = new Loonkost
                    {
                        Beschrijving = model.Beschrijving,
                        AantalUrenPerWeek = model.AantalUrenPerWeek,
                        BrutoMaandloonFulltime = dc.ConvertToDecimal(model.BrutoMaandloonFulltime),
                        Ondersteuningspremie = model.Ondersteuningspremie,
                        AantalMaandenIBO = model.AantalMaandenIBO,
                        IBOPremie = dc.ConvertToDecimal(model.IBOPremie)
                    };

                    if (model.doelgroep != null)
                    {
                        int doelgroepid = model.doelgroep ?? 0;
                        kost.Doelgroep = _doelgroepRepository.GetById(doelgroepid);
                    }

                    analyse.Loonkosten.Add(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolKost;
                }
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    TempData["error"] = e.Message;
                }
                else
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "LoonKosten", "VoegToe -- POST --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            try
            {
                Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, id);

                if (kost != null)
                {
                    LoonkostFormViewModel model = new LoonkostFormViewModel(kost, _doelgroepRepository.GetAll());

                    return PartialView("_Formulier", model);
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "LoonKosten", "Bewerk -- GET --"));
                _exceptionLogRepository.Save();

                TempData["error"] = Meldingen.OphalenFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, LoonkostViewModel model)
        {
            try
            {
                Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, model.Id);
                DecimalConverter dc = new DecimalConverter();
                if (ModelState.IsValid && kost != null)
                {
                    kost.Id = model.Id;
                    kost.Beschrijving = model.Beschrijving;
                    kost.AantalUrenPerWeek = model.AantalUrenPerWeek;
                    kost.BrutoMaandloonFulltime = dc.ConvertToDecimal(model.BrutoMaandloonFulltime);
                    kost.Doelgroep = _doelgroepRepository.GetById(model.Doelgroep.DoelgroepId);
                    kost.Ondersteuningspremie = model.Ondersteuningspremie;
                    kost.AantalMaandenIBO = model.AantalMaandenIBO;
                    kost.IBOPremie = dc.ConvertToDecimal(model.IBOPremie);

                    if (kost.Doelgroep == null)
                    {
                        TempData["error"] =
                            "Opgelet! U heeft nog geen doelgroep geselecteerd. Er zal dus nog geen resultaat " +
                            "berekend worden bij deze kost.";
                    }

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolKost;
                }
            }
            catch(Exception e)
            {
                if(e is ArgumentException || e is FormatException)
                {
                    ModelState.AddModelError("", e.Message);
                }else
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "LoonKosten", "Bewerk -- GET --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }             
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            try
            {
                Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, id);

                if (kost != null)
                {
                    analyse.Loonkosten.Remove(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "LoonKosten", "Verwijder"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.VerwijderFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private IEnumerable<LoonkostViewModel> MaakModel(Analyse analyse)
        {
            DecimalConverter dc = new DecimalConverter();
            return analyse
                .Loonkosten
                .Select(m => new LoonkostViewModel(m)
                {
                    Bedrag = analyse.Departement == null
                        ? ""
                        : dc.ConvertToString(m.BerekenTotaleLoonkost(analyse.Departement.Werkgever.AantalWerkuren,
                            analyse.Departement.Werkgever.PatronaleBijdrage))
                })
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.Loonkosten.Count == 0)
            {
                ViewData["totaalBrutolonen"] = 0;
                ViewData["totaalLoonkosten"] = 0;
            }

            if (analyse.Departement != null)
            {
                decimal totaal = LoonkostExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                    analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren,
                    analyse.Departement.Werkgever.PatronaleBijdrage);

                ViewData["totaalBrutolonen"] = totaal.ToString("C", new CultureInfo("nl-BE"));

                totaal = LoonkostExtensions.GeefTotaalAlleLoonkosten(
                    analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren,
                    analyse.Departement.Werkgever.PatronaleBijdrage);

                ViewData["totaalLoonkosten"] = totaal.ToString("C", new CultureInfo("nl-BE"));
            }
            else
            {
                ViewData["totaalBrutolonen"] = 0;
                ViewData["totaalLoonkosten"] = 0;
                TempData["error"] = "Opgelet! U heeft nog geen werkgever geselecteerd. Er zal dus nog geen resultaat " +
                                    "berekend worden bij deze kost.";
            }
        }
        #endregion
    }
}

