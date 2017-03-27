using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Controllers
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ResultaatController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public ResultaatController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            ResultaatViewModel model = new ResultaatViewModel();

            if (analyse.Departement != null)
            {
                IDictionary<Soort, double> kostenResultaat = analyse.GeefTotalenKosten();
                IDictionary<Soort, double> batenResultaat = analyse.GeefTotalenBaten();

                double kostenTotaal = kostenResultaat.Sum(t => t.Value);
                double batenTotaal = batenResultaat.Sum(t => t.Value);

                model.Resultaten = kostenResultaat;
                foreach(var rij in batenResultaat)
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
            
            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse)
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
    }
}
