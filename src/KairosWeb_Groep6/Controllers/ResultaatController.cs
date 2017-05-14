using System;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain.Excel;
using Microsoft.AspNetCore.Authorization;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class ResultaatController : Controller
    {
        #region Properties
        private readonly string outputDir = "temp\\";
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        #endregion

        #region Constructors
        public ResultaatController(IAnalyseRepository analyseRepository, IExceptionLogRepository exceptionLogRepository)
        {
            _analyseRepository = analyseRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }
        #endregion

        #region Index
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Index(Analyse analyse)
        {
            ResultaatViewModel model = new ResultaatViewModel();

            try
            {
                model.AnalyseKlaar = analyse.InArchief;
                model.AnalyseId = analyse.AnalyseId;

                if (analyse.Departement != null)
                {
                    IDictionary<Soort, decimal> kostenResultaat = analyse.GeefTotalenKosten();
                    IDictionary<Soort, decimal> batenResultaat = analyse.GeefTotalenBaten();

                    decimal kostenTotaal = kostenResultaat.Sum(t => t.Value);
                    decimal batenTotaal = batenResultaat.Sum(t => t.Value);

                    model.Resultaten = kostenResultaat;
                    foreach (var rij in batenResultaat)
                    {
                        model.Resultaten.Add(rij);
                    }

                    model.KostenTotaal = kostenTotaal;
                    model.BatenTotaal = batenTotaal;
                    model.Totaal = batenTotaal - kostenTotaal;

                    ViewData["SubTotaalBaten"] = model.BatenTotaal;
                   
                   
                    // kleur voor nettoresultaat bepalen
                    if (model.Totaal < 0)
                    {
                        ViewData["klasseTotaal"] = "alert-danger";
                    }
                    else if (model.Totaal == 0)
                    {
                        ViewData["klasseTotaal"] = "alert-warning";
                    }
                    else
                    {
                        ViewData["klasseTotaal"] = "alert-success";
                    }
                }
                else
                {
                    TempData["error"] =
                            "Opgelet! U heeft nog geen werkgever geselecteerd. Er zal dus nog geen resultaat " +
                            "berekend worden voor deze analyse.";
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Resultaat", "Index"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
            }


            return View(model);
        }
        #endregion

        #region Opslaan
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Opslaan(Analyse analyse)
        {
            try
            {
                analyse.UpdateTotalen(_analyseRepository);
                _analyseRepository.Save();

                TempData["message"] = "De analyse is succesvol opgeslaan.";
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Resultaat", "Opslaan"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw.";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region MaakExcel
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult MaakExcel(Jobcoach jobcoach, int id)
        {
            try
            {
                // eerst kijken of deze analyse wel van deze jobcoach is
                Analyse mogelijkeAnalyse = jobcoach.Analyses.SingleOrDefault(a => a.AnalyseId == id);

                if (mogelijkeAnalyse == null)
                {
                    TempData["error"] = "U heeft geen toegang tot deze analyse! Open enkel analyses die u ziet " +
                                        "op de homepagina of in het archief.";
                }
                else
                {
                    Analyse analyse = _analyseRepository.GetById(id);
                    ExcelWriterResultaat excelWriter = new ExcelWriterResultaat();
                    string fileName = excelWriter.MaakExcel(analyse);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(outputDir + fileName);

                    // bestand terug verwijderen van de server
                    excelWriter.VerwijderBestand();

                    return File(fileBytes, "application/x-msdownload", fileName);
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Resultaat", "MaakExcel"));
                _exceptionLogRepository.Save();
                TempData["error"] =
                    "Er ging iets fout tijdens het samenstellen van het Excel-bestand, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Mailen
        public IActionResult Mail(int id)
        {
            try
            {
                ResultaatMailViewModel model = new ResultaatMailViewModel{AnalyseId = id};

                return View(model);
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Resultaat", "Mail -- GET --"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging onverwacht iets fout, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ServiceFilter(typeof(JobcoachFilter))]
        public async Task<IActionResult> Mail(Jobcoach jobcoach, ResultaatMailViewModel model)
        {
            try
            {
                // eerst kijken of deze analyse wel van deze jobcoach is
                Analyse mogelijkeAnalyse = jobcoach.Analyses.SingleOrDefault(a => a.AnalyseId == model.AnalyseId);

                if (mogelijkeAnalyse == null)
                {
                    TempData["error"] = "U heeft geen toegang tot deze analyse! Open enkel analyses die u ziet " +
                                        "op de homepagina of in het archief.";
                }
                else
                {
                    Analyse analyse = _analyseRepository.GetById(model.AnalyseId);
                    ExcelWriterResultaat excelWriter = new ExcelWriterResultaat();
                    string fileName = excelWriter.MaakExcel(analyse);
                    FileInfo file = new FileInfo("temp\\" + fileName);
                    string naam = model.Voornaam + " " + model.Naam;

                    bool gelukt = await EmailSender.SendResultaat(naam, model.Emailadres, model.Onderwerp, model.Bericht, file);

                    // bestand terug verwijderen van de server
                    excelWriter.VerwijderBestand();

                    if (gelukt)
                    {
                        TempData["message"] = "Het resultaat is succesvol verzonden";
                    }
                    else
                    {
                        TempData["error"] =
                            "Het is op dit moment niet mogelijk om mails te verzenden, probeer later opnieuw";
                    }
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Resultaat", "Mail -- POST --"));
                _exceptionLogRepository.Save();
                TempData["error"] =
                    "Er ging iets fout tijdens het verzenden van het resultaat, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region AnalyseKlaar
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult AnalyseKlaar(Analyse analyse)
        {
            try
            {
                analyse.InArchief = !analyse.InArchief;
                _analyseRepository.Save();

                if (analyse.InArchief)
                {
                    TempData["message"] = "De analyse is succesvol gemarkeerd als 'Klaar'";
                }
                else
                {
                    TempData["message"] = "De analyse is succesvol gemarkeerd als 'Nog niet klaar'";
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Resultaat", "AnalyseKlaar"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging onverwachts iets fout tijdens het opslaan van de wijzigingen aan de analyse, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
