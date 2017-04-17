using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using KairosWeb_Groep6.Models.Domain.Excel;

namespace KairosWeb_Groep6.Controllers
{
    public class ResultaatController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        //private System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
        public ResultaatController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Index(Analyse analyse)
        {
            ResultaatViewModel model = new ResultaatViewModel();

            try
            {
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
            catch
            {
                TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw";
            }
            
            return View(model);
        }
        #endregion

        #region Opslaan
        public IActionResult Opslaan(int id)
        {
            try
            {
                _analyseRepository.Save();

                // Pas als er geen exception geweest is, de sessionvariabele verwijderen
                AnalyseFilter.ClearSession(HttpContext);
                TempData["message"] = "De analyse is succesvol opgeslaan.";
            }
            catch
            {
                TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw.";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region MaakExcel

        public IActionResult MaakExcel(int id)
        {
            try
            {
                Analyse analyse = _analyseRepository.GetById(id);
                ExcelWriterResultaat excelWriter = new ExcelWriterResultaat();
                string fileName = excelWriter.MaakExcel(analyse);
                byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
                
                // bestand terug verwijderen van de server
                excelWriter.VerwijderBestand();

                return File(fileBytes, "application/x-msdownload", fileName);
            }
            catch
            {
                TempData["error"] =
                    "Er ging iets fout tijdens het samenstellen van het Excel-bestand, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Afdrukken
        public IActionResult Afdrukken(int id)
        {
            //try
            //{
            //    Analyse analyse = _analyseRepository.GetById(id);
            //    ExcelWriterResultaat excelWriter = new ExcelWriterResultaat();
            //    string fileName = excelWriter.MaakExcel(analyse);
            //    byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);

            //    // bestand terug verwijderen van de server
            //    excelWriter.VerwijderBestand();

            //    return File(fileBytes, "application/x-msdownload", fileName);
            //}
            //catch
            //{
            //    TempData["error"] =
            //        "Er ging iets fout tijdens het laden van het afdrukscherm, probeer later opnieuw";
            //}
            TempData["message"] = "Deze functionaliteit wordt nog niet ondersteund";

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
            catch
            {
                TempData["error"] = "Er ging onverwacht iets fout, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Mail(ResultaatMailViewModel model)
        {
            try
            {
                Analyse analyse = _analyseRepository.GetById(model.AnalyseId);
                ExcelWriterResultaat excelWriter = new ExcelWriterResultaat();
                string fileName = excelWriter.MaakExcel(analyse);
                FileInfo file = new FileInfo(fileName);
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

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["error"] =
                    "Er ging iets fout tijdens het verzenden van het resultaat, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
