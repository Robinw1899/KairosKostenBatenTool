using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Controllers
{
    public class ResultaatController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

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
    }
}
